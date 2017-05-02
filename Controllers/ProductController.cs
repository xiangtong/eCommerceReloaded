using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eCommerceReloaded.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace eCommerceReloaded.Controllers
{
   public class ProductController : Controller
    {
        private eCommerceReloadedContext _context;
        private readonly IHostingEnvironment _env;
        public ProductController(eCommerceReloadedContext context,IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    
        [HttpGet]
        [Route("/admin")]
        public IActionResult ProductAdmin2()
        {
            List<Product> products =_context.products
                    .Include(p=>p.category)
                    .OrderByDescending(c=>c.created_At)
                    .ToList();
            List<Category> categories =_context.categories.ToList();
            List<Event> events= _context.events
                    .OrderBy(e=>e.month)
                    .ToList();
            List<ProductEvent> productevents=_context.productEvents
                    .Include(p=>p.anEvent)
                    .OrderByDescending(p=>p.productId)
                    .ToList();
            ViewBag.categories=categories;
            ViewBag.events=events;
            ViewBag.products=products;
            ViewBag.productevents=productevents;
            if(TempData["error"]!=null)
            {
                ViewData["error"]=TempData["error"];
            }
            return View("productadmin2");
        }
        // Post: add a new category
        [HttpPost]
        [Route("/admin/category/new")]
        public IActionResult NewCategory(string name)
        {
            Category newcategory=new Category();
            newcategory.name=name;
            _context.Add(newcategory);
            _context.SaveChanges();
            return RedirectToAction("ProductAdmin2");          
        }
        // Post: add a new event
        [HttpPost]
        [Route("/admin/event/new")]
        public IActionResult NewEvent(string name,int month)
        {
            Event newevent=new Event();
            newevent.name=name;
            newevent.month=month;
            _context.Add(newevent);
            _context.SaveChanges();
            return RedirectToAction("ProductAdmin2");          
        }
        // Post : add a new product
        [HttpPost]
        [Route("/admin/product/new")]
        public IActionResult NewProduct(string name,int inventory,int price,string description, int category, IList<IFormFile> image, int[]  Event )
        {
            if(ModelState.IsValid)
            {
                Product product = _context.products
                    .SingleOrDefault(c=>c.name==name);
                if(product !=null)
                {
                    TempData["error"]="Product name has existed!";
                }
                else
                {
                    long size = 0;
                    string imageUrl="";
                    foreach(var file in image)
                        {
                            var fileName = ContentDispositionHeaderValue
                                            .Parse(file.ContentDisposition)
                                            .FileName
                                            .Trim('"');
                            imageUrl=Convert.ToString(fileName);
                            string filename = _env.WebRootPath + $@"\images\{fileName}";
                            size += file.Length;
                            using (FileStream fs = System.IO.File.Create(filename))
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    Product newproduct=new Product();
                    newproduct.created_At=DateTime.Now;
                    newproduct.name=name;
                    newproduct.inventory=inventory;
                    newproduct.description=description;
                    newproduct.price=price;
                    newproduct.categoryId=category;
                    newproduct.imageUrl="/images/"+imageUrl;
                    _context.Add(newproduct);
                    _context.SaveChanges();
                    int pid=newproduct.productId;
                    for(int i=0;i<Event.Length;i++)
                    {
                        ProductEvent newpie =new ProductEvent();
                        newpie.productId=pid;
                        newpie.eventId= Event[i];
                        _context.Add(newpie);
                        _context.SaveChanges();
                    }
                }
                return RedirectToAction("ProductAdmin2");
            }       
            List<Product> products =_context.products
                    .OrderByDescending(c=>c.created_At)
                    .ToList();
            List<Category> categories =_context.categories.ToList();
            List<Event> events= _context.events.ToList();
            ViewBag.categories=categories;
            ViewBag.events=events;
            ViewBag.products=products;
            return View("productadmin2");
        }
    }
}
