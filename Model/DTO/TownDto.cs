using Microsoft.AspNetCore.Http;
using MyApi.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyApi1.Model.DTO
{
    public class TownDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Peyment { get; set; }
        public string VidUrl { get; set; }
        public double Lang { get; set; }
        public double Lat { get; set; }
        public string Mantaqe { get; set; }
        public IFormFile UtmPic { get; set; }
        public int CityId { get; set; }
        public int ProvinceId { get; set; }
        //public List<GallerySharedDto> GalleryPics { get; set; }
        public IFormFileCollection GalleryPics { get; set; }
    }
    public class TownSelectDto
    {
        public TownSelectDto()
        {
            Pics = new List<string>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Peyment { get; set; }
        public string Desc { get; set; }
        public string VidUrl { get; set; }
        public double Lat { get; set; }
        public double Lang { get; set; }
        public string CityName { get; set; }
        public string UtmPic { get; set; }
        //public int PicturesCount { get; set; }
        public int CityId { get; set; }
        public CityDetail City { get; set; }
        public List<TownGallery> GalleryPics { get; set; }
        public List<string> Pics { get; set; }
    }
    public class TownCommonModelAsync
    {
        public TownCommonModelAsync()
        {
            PageSize = 10;
            Page = 1;
        }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public CancellationToken CancellationToken { get; set; }

        //public string Search { get; set; }
        //public Search SearchFilter { get; set; }

        //public OrderSort? Order { get; set; }
    }
}
