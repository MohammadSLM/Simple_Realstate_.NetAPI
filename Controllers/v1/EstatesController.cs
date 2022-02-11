using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Model;
using MyApi1.Model;
using MyApi1.Model.DTO;
using static MyApi1.Extensions;

namespace MyApi1.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EstatesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EstatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Estates
        [HttpGet("[action]")]
        public async Task<ActionResult<List<EstateSelectDto>>> GetEstates([FromQuery] EstateCommonModelAsync model)
        {
            var list = _context.Estates.AsNoTracking();

            //search, filter and sorting
            list = SearchFilter(list, model);

            var result = await list
                .Skip((model.Page - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(a => new EstateSelectDto
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
                    City = new CityDetail
                    {
                        CityId = a.CityId,
                        ProvinceId = a.ProvinceId,
                        CityTitle = _context.City.SingleOrDefault(x => x.Id == a.CityId).Title,
                        ProvinceTitle = _context.City.SingleOrDefault(x => x.Id == a.ProvinceId).Title
                    },
                    VidUrl = a.VidUrl,
                    Price = a.Price,
                    CityName = a.City.Title,
                    GalleryPics = _context.EstateGalleries.Where(b => b.EstateId == a.Id).ToList()
                }).OrderByDescending(a => a.Id)
                .ToListAsync();

            foreach (var item in result)
            {
                foreach (var pic in item.GalleryPics)
                {
                    var url = BaseConfiguration.GetBaseUrl() + pic.PicUrl;
                    item.Pics.Add(url);
                    pic.PicUrl = BaseConfiguration.GetBaseUrl() + pic.PicUrl;
                }
            }

            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<EstateSelectDto>>> GetEstatesBy9()
        {
            var list = await _context.Estates.Select(a => new EstateSelectDto
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
                City = new CityDetail
                {
                    CityId = a.CityId,
                    ProvinceId = a.ProvinceId,
                    CityTitle = _context.City.SingleOrDefault(x => x.Id == a.CityId).Title,
                    ProvinceTitle = _context.City.SingleOrDefault(x => x.Id == a.ProvinceId).Title
                },
                VidUrl = a.VidUrl,
                Price = a.Price,
                CityName = a.City.Title,
                GalleryPics = _context.EstateGalleries.Where(b => b.EstateId == a.Id).ToList()

            }).OrderByDescending(a => a.Id)
                .Take(9)
                .ToListAsync();

            foreach (var item in list)
            {
                foreach (var pic in item.GalleryPics)
                {
                    var url = BaseConfiguration.GetBaseUrl() + pic.PicUrl;
                    item.Pics.Add(url);
                    pic.PicUrl = BaseConfiguration.GetBaseUrl() + pic.PicUrl;
                }
            }

            return list;
        }

        // GET: api/Estates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estate>> GetEstate(int id, CancellationToken cancellationToken)
        {
            var res = await _context.Estates.AsNoTracking().Select(a => new EstateSelectDto
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
                    CityTitle = _context.City.SingleOrDefault(x => x.Id == a.CityId).Title,
                    ProvinceTitle = _context.City.SingleOrDefault(x => x.Id == a.ProvinceId).Title
                },
                GalleryPics = _context.EstateGalleries.Where(b => b.EstateId == a.Id).ToList()

            }).SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

            foreach (var pic in res.GalleryPics)
            {
                var url = BaseConfiguration.GetBaseUrl() + pic.PicUrl;
                res.Pics.Add(url);
                pic.PicUrl = BaseConfiguration.GetBaseUrl() + pic.PicUrl;
            }


            if (res == null)
            {
                return NotFound();
            }

            return Ok(res);
        }

        // PUT: api/Estates/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] EstateDto dto, int id)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var model = await _context.Estates.Include(a => a.EstateGalleries).SingleOrDefaultAsync(a => a.Id == id);
            if (model == null) return NotFound();

            model.Id = dto.Id;
            model.Title = dto.Title;
            model.Desc = dto.Desc;
            model.Code = dto.Code;
            model.Bedroom = dto.Bedroom;
            model.BuildingArea = dto.BuildingArea;
            model.CityId = dto.CityId;
            model.ProvinceId = dto.ProvinceId;
            model.LandArea = dto.LandArea;
            model.Lang = dto.Lang;
            model.Lat = dto.Lat;
            model.Peyment = dto.Peyment;
            model.Price = dto.Price;
            model.Region = dto.Region;
            model.VidUrl = dto.VidUrl;

            #region Full MultiFile Upload

            //var oldPics = model.EstateGalleries.Where(a => a.EstateId == model.Id).Select(a => a.PicUrl).ToList();
            //var newPics = dto.GalleryPics.ToList();
            //List<string> newPicStrings = new List<string>();
            //foreach (var item in newPics)
            //{
            //    var newPicUrl = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Estate", item.FileName);
            //    newPicStrings.Add(newPicUrl);
            //}

            //foreach (var oldp in oldPics)
            //{
            //    if (!newPicStrings.Contains(oldp))
            //    {
            //        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + oldp.TrimStart('\\'));
            //        FileInfo myfileinf = new FileInfo(path);
            //        myfileinf.Delete();

            //        var galleryPic = await _context.EstateGalleries.SingleOrDefaultAsync(a => a.EstateId == model.Id);
            //        _context.EstateGalleries.Remove(galleryPic);
            //    }
            //}

            //foreach (var newp in dto.GalleryPics)
            //{
            //    var newPicUrl = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Estate", newp.FileName);
            //    if (!oldPics.Contains(newPicUrl))
            //    {
            //        using (var Stream = new FileStream(newPicUrl, FileMode.Create))
            //        {
            //            await newp.CopyToAsync(Stream);
            //        }

            //        EstateGallery estateGallery = new EstateGallery()
            //        {
            //            EstateId = model.Id,
            //            PicUrl = "/Upload/Estate/" + newp.FileName
            //        };
            //        _context.EstateGalleries.Add(estateGallery);
            //    }
            //}
            #endregion


            _context.Estates.Update(model);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Estates
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<EstateDto>> Add([FromForm] EstateDto dto,CancellationToken cancellationToken)
        {
            Estate estate = new Estate()
            {
                Title = dto.Title,
                Desc = dto.Desc,
                Region = dto.Region,
                Bedroom = dto.Bedroom,
                BuildingArea = dto.BuildingArea,
                LandArea = dto.LandArea,
                Peyment = dto.Peyment,
                Price = dto.Price,
                Code = dto.Code,
                VidUrl = dto.VidUrl,
                Lat = dto.Lat,
                Lang = dto.Lang,
                CityId = dto.CityId,
                ProvinceId = dto.ProvinceId
            };
            await _context.Estates.AddAsync(estate,cancellationToken);
            await _context.SaveChangesAsync();

            if (dto.GalleryPics.Any())
            {
                foreach (var item in dto.GalleryPics)
                {
                    if (item != null && item.Length > 0)
                    {
                        string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Estate", item.FileName);
                        using (var Stream = new FileStream(uploadpath, FileMode.Create))
                        {
                            await item.CopyToAsync(Stream);
                        }

                        EstateGallery estateGallery = new EstateGallery()
                        {
                            EstateId = estate.Id,
                            PicUrl = "/Upload/Estate/" + item.FileName
                        };
                        await _context.EstateGalleries.AddAsync(estateGallery,cancellationToken);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return Ok();
        }

        // DELETE: api/Estates/5
        [HttpDelete("{id}")]
        //[Authorize]
        public async Task<ActionResult<Estate>> Delete(int id)
        {
            var estate = await _context.Estates.FindAsync(id);
            if (estate == null)
            {
                return NotFound();
            }

            _context.Estates.Remove(estate);
            var gallery = _context.EstateGalleries.Where(a => a.EstateId.Equals(id)).ToList();

            foreach (var item in gallery)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + item.PicUrl.TrimStart('\\'));
                FileInfo myfileinf = new FileInfo(path);
                myfileinf.Delete();

                _context.EstateGalleries.Remove(item);
            }
            await _context.SaveChangesAsync();

            return estate;
        }

        private bool EstateExists(int id)
        {
            return _context.Estates.Any(e => e.Id == id);
        }
        private IQueryable<Estate> SearchFilter(IQueryable<Estate> list, EstateCommonModelAsync model)
        {
            #region SearchFilter and Sorting
            if (model.SearchFilter != null)
            {
                if (!string.IsNullOrWhiteSpace(model.SearchFilter.Title))
                    list = list.Where(_ => _.Title.Contains(model.SearchFilter.Title));

                if (model.SearchFilter.City != null && !model.SearchFilter.City.Contains(0)) list = list.Where(_ => model.SearchFilter.City.Contains(_.CityId));

                if (model.SearchFilter.Province != 0) list = list.Where(_ => _.ProvinceId == model.SearchFilter.Province);

                if (model.SearchFilter.BuildingTypeSearch != null)
                {
                    if (model.SearchFilter.BuildingTypeSearch == BuildingType.Villa) list = list.Where(_ => _.BuildingType == BuildingType.Villa);
                    else if (model.SearchFilter.BuildingTypeSearch == BuildingType.Apartment) list = list.Where(_ => _.BuildingType == BuildingType.Apartment);
                    else if (model.SearchFilter.BuildingTypeSearch == BuildingType.Land) list = list.Where(_ => _.BuildingType == BuildingType.Land);
                    else if (model.SearchFilter.BuildingTypeSearch == BuildingType.Tower) list = list.Where(_ => _.BuildingType == BuildingType.Tower);
                }

                if (model.SearchFilter.StartPrice != 0
                    && model.SearchFilter.EndPrice != 0)
                {
                    list = list.Where(_ => _.Price >= model.SearchFilter.StartPrice
                      && _.Price <= model.SearchFilter.EndPrice);
                }
            }
            #endregion

            #region Sort
            if (model.Order != null)
            {
                switch (model.Order)
                {
                    case OrderSort.Newer:
                        list = list.OrderByDescending(_ => _.Id);
                        break;
                    case OrderSort.Cheap:
                        list = list.OrderBy(_ => _.Price);
                        break;
                    case OrderSort.Expensive:
                        list = list.OrderByDescending(_ => _.Price);
                        break;
                }
            }
            else list = list.OrderByDescending(_ => _.Id);
            #endregion

            return list;
        }
    }
}
