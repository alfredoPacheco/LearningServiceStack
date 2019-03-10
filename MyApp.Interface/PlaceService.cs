using MyApp.Model;
using ServiceStack;
using ServiceStack.OrmLite;
using System.Linq;
using System.Net;

namespace MyApp.Interface
{
    public class PlaceService : Service,
        IGet<PlaceToVisit>,
        IGet<AllPlacesToVisit>,
        IPost<CreatePlaceToVisit>,
        IPut<UpdatePlaceToVisit>,
        IDelete<DeletePlaceToVisit>
    {
        public object Get(PlaceToVisit request)
        {
            if (!Db.Exists<Place>(x => x.Id == request.Id))
            {
                throw HttpError.NotFound("Place not found");
            }
            return new PlaceToVisitResponse
            {
                Place = Db.SingleById<Place>(request.Id)
            };
        }

        public object Get(AllPlacesToVisit request)
        {
            return new AllPlacesToVisitResponse
            {
                Places = Db.Select<Place>().ToList()
            };
        }

        public object Post(CreatePlaceToVisit request)
        {
            var place = request.ConvertTo<Place>();
            Db.Insert(place);
            return new PlaceToVisitResponse
            {
                Place = place
            };
        }

        public object Put(UpdatePlaceToVisit request)
        {
            if (!Db.Exists<Place>(x => x.Id == request.Id))
            {
                throw HttpError.NotFound("Place not found");
            }
            var place = request.ConvertTo<Place>();
            Db.Update(place);
            return new PlaceToVisitResponse
            {
                Place = place
            };
        }

        public object Delete(DeletePlaceToVisit request)
        {
            if (!Db.Exists<Place>(x => x.Id == request.Id))
            {
                throw HttpError.NotFound("Place not found");
            }
            Db.DeleteById<Place>(request.Id);
            base.Response.StatusCode = (int)HttpStatusCode.NoContent;
            return null;
        }
    }
}
