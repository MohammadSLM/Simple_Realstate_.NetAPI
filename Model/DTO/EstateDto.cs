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
    public class EstateDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Region { get; set; }
        public int LandArea { get; set; }
        public int BuildingArea { get; set; }
        public int Bedroom { get; set; }
        public string Peyment { get; set; }
        public int Price { get; set; }
        public int Code { get; set; }
        public double Lat { get; set; }
        public double Lang { get; set; }
        public string Desc { get; set; }
        public string VidUrl { get; set; }
        public BuildingType BuildingType { get; set; }
        public int CityId { get; set; }
        public int ProvinceId { get; set; }
        //public List<GallerySharedDto> GalleryPics { get; set; }
        public IFormFileCollection GalleryPics { get; set; }
    }

    public class EstateSelectDto
    {
        public EstateSelectDto()
        {
            Pics = new List<string>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Region { get; set; }
        public int LandArea { get; set; }
        public int BuildingArea { get; set; }
        public int Bedroom { get; set; }
        public string Peyment { get; set; }
        public int Price { get; set; }
        public int Code { get; set; }
        public double Lat { get; set; }
        public double Lang { get; set; }
        public string Desc { get; set; }
        public string VidUrl { get; set; }
        public BuildingType BuildingType { get; set; }
        public string CityName { get; set; }
        //public int PicturesCount { get; set; }
        public CityDetail City { get; set; }
        public List<EstateGallery> GalleryPics { get; set; }
        public List<String> Pics { get; set; }
    }
    public class EstateCommonModelAsync
    {
        public EstateCommonModelAsync()
        {
            PageSize = 10;
            Page = 1;
        }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public CancellationToken CancellationToken { get; set; }

        //public string Search { get; set; }
        public Search SearchFilter { get; set; }

        public OrderSort? Order { get; set; }
    }
    public class Search
    {
        public string Title { get; set; }
        public int[] City { get; set; }
        public int Province { get; set; }
        public float StartBuildingAgeNumber { get; set; }
        public float EndBuildingAgeNumber { get; set; }
        public bool? JustBuildingAgeIsNew { get; set; }
        public long StartPrice { get; set; }
        public long EndPrice { get; set; }
        public BuildingType? BuildingTypeSearch { get; set; }
    }

    //public enum BuildingTypeSearch
    //{
    //    /// <summary>
    //    /// ویلایی
    //    /// </summary>
    //    [Display(Name = "ویلایی")]
    //    Villa = 0,

    //    /// <summary>
    //    /// شهری
    //    /// </summary>
    //    [Display(Name = "شهری")]
    //    Urban = 1
    //}

    public enum OrderSort
    {

        /// <summary>
        /// جدیدترین
        /// </summary>
        Newer = 0,

        /// <summary>
        /// ارزان ترین
        /// </summary>
        Cheap = 1,

        /// <summary>
        /// گران ترین
        /// </summary>
        Expensive = 2
    }
}
