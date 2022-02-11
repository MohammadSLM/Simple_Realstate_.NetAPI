using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Model;
using MyApi1.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MyApi1.Extensions;

namespace MyApi1.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IndexDto>> Index()
        {
            var model = new IndexDto
            {
                CustomersCount = await _context.Customer.CountAsync(),
                EstatesCount = await _context.Estates.CountAsync(),
                MembersCount = await _context.Members.CountAsync(),
                ProjectsCount = await _context.Project.CountAsync(),
                SlidersCount = await _context.Sliders.CountAsync(),
                TownsCount = await _context.Towns.CountAsync(),
                ZonesCount = await _context.Zones.CountAsync(),
                Estate5 = await _context.Estates.TakeLast(5).Select(a => new EstateSelectDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Bedroom = a.Bedroom,
                    BuildingArea = a.BuildingArea,
                    LandArea = a.LandArea,
                    Code = a.Code,
                    Desc = a.Desc,
                    Lat = a.Lat,
                    Lang = a.Lang,
                    Peyment = a.Peyment,
                    Region = a.Region,
                    VidUrl = a.VidUrl,
                    Price = a.Price,
                    CityName = a.City.Title,
                    City = new CityDetail
                    {
                        CityId = a.CityId,
                        ProvinceId = a.ProvinceId,
                        CityTitle = a.City.Title,
                        ProvinceTitle = a.Province.Title
                    },
                    GalleryPics = _context.EstateGalleries.Where(b => b.EstateId == a.Id).ToList()

                }).ToListAsync(),

                Town5 = await _context.Towns.TakeLast(5).Select(a => new TownSelectDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Desc = a.Desc,
                    Lang = a.Lang,
                    Lat = a.Lat,
                    Peyment = a.Peyment,
                    VidUrl = a.VidUrl,
                    UtmPic = a.UtmPicUrl,
                    City = new CityDetail
                    {
                        CityId = a.CityId,
                        ProvinceId = a.ProvinceId,
                        CityTitle = _context.City.SingleOrDefault(x => x.Id == a.CityId).Title,
                        ProvinceTitle = _context.City.SingleOrDefault(x => x.Id == a.ProvinceId).Title
                    },
                    GalleryPics = _context.TownGalleries.Where(b => b.TownId == a.Id).ToList(),
                }).ToListAsync(),
                Zones5 = await _context.Zones.TakeLast(5).Select(a => new ZoneSelectDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Area = a.Area,
                    Desc = a.Desc,
                    VidUrl = a.VidUrl,
                    CityId = a.CityId,
                    ProvinceId = _context.City.Where(b => b.Id == _context.City.Where(z => z.Id == a.CityId).Select(m => m.ParentId).SingleOrDefault()).Select(c => c.Id).SingleOrDefault(),
                    Pic = BaseConfiguration.GetBaseUrl() + a.PicUrl

                }).ToListAsync()
        };

            foreach (var item in model.Estate5)
            {
                foreach (var pic in item.GalleryPics)
                {
                  pic.PicUrl = BaseConfiguration.GetBaseUrl() + pic.PicUrl;
                }
            }
            foreach (var item in model.Town5)
            {
                foreach (var pic in item.GalleryPics)
                {
                    pic.PicUrl = BaseConfiguration.GetBaseUrl() + pic.PicUrl;
                }
            }
            return model;
        }
    }
}
