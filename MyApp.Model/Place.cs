using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Model
{
    public class Place
    {
        [Index]
        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }

    [Route("/places/{Id}", Verbs = "GET")]
    public class PlaceToVisit : IReturn<PlaceToVisitResponse>
    {
        public long Id { get; set; }
    }

    [Route("/places", Verbs = "GET")]
    public class AllPlacesToVisit : IReturn<AllPlacesToVisitResponse>
    {
    }

    [Route("/places", Verbs = "POST")]
    public class CreatePlaceToVisit : IReturn<CreatePlaceToVisitResponse>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    [Route("/places/{Id}", Verbs = "PUT")]
    public class UpdatePlaceToVisit : IReturn<UpdatePlaceToVisitResponse>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    [Route("/places/{Id}", Verbs = "DELETE")]
    public class DeletePlaceToVisit
    {
        public long Id { get; set; }
    }
    public class PlaceToVisitResponse
    {
        public Place Place { get; set; }
    }

    public class AllPlacesToVisitResponse
    {
        public List<Place> Places { get; set; }
    }
    public class CreatePlaceToVisitResponse
    {
        public Place Place { get; set; }
    }
    public class UpdatePlaceToVisitResponse
    {
        public Place Place { get; set; }
    }
}
