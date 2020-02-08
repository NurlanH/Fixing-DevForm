﻿using DevFormAz.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevFormAz.Models;
using DevFormAz.DevFormData;
using DevFormAz.Extentions;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web.Helpers;
using PagedList;
using PagedList.Mvc;

namespace DevFormAz.Controllers
{
    public class HomeController : Controller
    {
        readonly DevFormAzDataBase db = new DevFormAzDataBase();
        readonly FormMethods helperMethods = new FormMethods();
        readonly UserMethods helperUserMethod = new UserMethods();

        //Home page
        public ActionResult Index()
        {
            return View();
        }

        //Form page
        public ActionResult FormPage(int? page)
        {
            

            FormTagViewModel formVm = new FormTagViewModel()
            {
                Forms = db.Forms.OrderByDescending(i=>i.Id).ToList(),
                TagLists = db.TagLists.ToList()
            };
            return View(formVm);
        }

        [HttpPost]
        public ActionResult FormPage(Form form, string tagname, List<HttpPostedFileBase> formImg)
        {

            var userId = (int)Session["UserId"];
            if (ModelState.IsValid)
            {
                if (form != null)
                {
                    
                    var tagArr = tagname.Split(',');


                    Form newForm = new Form() {
                        Name = form.Name,
                        WhenIsDeleted = DateTime.Now,
                        FormTime = DateTime.Now,
                        Description = form.Description,
                        UserId = (int)Session["UserId"]
                    };
                    db.Forms.Add(newForm);
                    db.SaveChanges();


                    // For tag list
                    for (int i = 0; i < tagArr.Length; i++)
                    {
                        if (tagArr[i] != null && tagArr[i] != "" && tagArr[i] != " ")
                        {
                            TagList tag = new TagList()
                            {
                                TagName = tagArr[i],
                                FormId = newForm.Id
                            };
                           
                            db.TagLists.Add(tag);
                        }
                    }


                    // For form image
                    foreach (var img in formImg)
                    {
                        if (img != null)
                        {
                            if (İmageControl.CheckImageSize(img, 4))
                            {

                                var formImages = İmageControl.SaveImage(Server.MapPath("~/Public/Images/PostImgs"), img);
                                FormImage fImg = new FormImage()
                                {
                                    FormId = newForm.Id,
                                    ImageName = formImages
                                };
                                db.FormImages.Add(fImg);
                            };

                        }
                    }

                    var mySubs = db.Subscribes.Where(f => f.FollowId == userId).Select(fo => fo.FollowerId).ToList();
                    var myInfo = db.Users.Find(userId);
                    db.SaveChanges();
                    helperMethods.SendNotificationAsync(mySubs, myInfo);
                    return RedirectToAction("FormPage","Home");
                }
            }
            return View();
        }


        //Forn view page
        public ActionResult FormView(int? id)
        {
            if (id != null)
            {
                var findForm = db.Forms.Find(id);
                findForm.TagLists = db.TagLists.Where(f => f.FormId == (int)id).ToList();

                if (findForm != null && findForm.isDeleted != true)
                {
                    if ((int?)Session["UserId"] != null)
                    {
                        var userId = (int)Session["UserId"];


                        var checkUserView = db.FormViewCounts.Any(u => u.UserId == userId && u.FormId == id);
                        if (!checkUserView)
                        {
                            FormViewCount Vcount = new FormViewCount()
                            {
                                FormId = (int)id,
                                UserId = (int)Session["UserId"]

                            };
                            db.FormViewCounts.Add(Vcount);
                            db.SaveChanges();
                        }
                    }
                    
                    return View(findForm);

                }
                else
                {
                    return RedirectToAction("FormPage", "Home");
                }


            }
            else
            {
                return RedirectToAction("FormPage", "Home");
            }

        }

        //Edit user Form
        public ActionResult EditForm(int? id)
        {
            if (id != null)
            {
                var checkUser = (int?)Session["UserId"];

                if (db.Forms.Find(id).UserId == checkUser || db.Users.Find(checkUser).UserDetail.Role == "admin")
                {
                    EditFormVM ftVm = new EditFormVM()
                    {
                        Form = db.Forms.Find(id),
                        TagLists = db.TagLists.Where(t => t.FormId == id).ToList()
                    };
                    return View(ftVm);
                }
                else
                {
                    return RedirectToAction("FormPage", "Home");
                }
            }
            else
            {
                return RedirectToAction("FormPage", "Home");
            }

        }

        //Edit user Form post
        [HttpPost]
        public ActionResult EditForm(Form form, List<HttpPostedFileBase> addImg, string deleteImg, int? id, string existTag, string editTag)
        {
            var userId = (int?)Session["UserId"];

            if (userId != null && id != null)
            {
                var myForm = db.Forms.Where(f => f.Id == id && f.UserId == userId).SingleOrDefault();
                var delImg = deleteImg.Split(',');

                if (ModelState.IsValid && myForm.isDeleted == false)
                {

                    myForm.Name = form.Name;
                    myForm.Description = form.Description;

                    //Edit or Add tag
                    helperMethods.EditFrmTag(myForm.Id, editTag, existTag);


                    //Add img form
                    foreach (var item in addImg)
                    {
                        if (item != null)
                        {
                            if (İmageControl.CheckImageSize(item, 4))
                            {
                                var image = İmageControl.SaveImage(Server.MapPath("~/Public/Images/PostImgs"), item);
                                FormImage fImg = new FormImage()
                                {
                                    FormId = myForm.Id,
                                    ImageName = image
                                };
                                db.FormImages.Add(fImg);
                            }

                        };

                    };

                    //Delete img form
                    foreach (var item in delImg)
                    {
                        if (item != null && item != "")
                        {
                            db.FormImages.Remove(myForm.FormImages.Where(fd => fd.ImageName == item).SingleOrDefault());
                            İmageControl.DeleteImage(Server.MapPath("~/Public/Images/PostImgs"), item);
                        }
                    };

                    db.SaveChanges();
                    return RedirectToAction("FormView", "Home", new { myForm.Id });
                }
                else
                {
                    return RedirectToAction("EditForm", "Home", new { myForm.Id });
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        //DeleteForm
        public ActionResult DeleteForm(int? id)
        {
            var userId = (int?)Session["UserId"];
            if (id != null && userId != null)
            {
                helperMethods.DeleteForm((int)id, (int)userId);
            }
            return RedirectToAction("FormPage", "Home");
        }

        //Tag page
        public ActionResult TagPage()
        {
            return View(db.TagLists.ToList());
        }

        //Profile page
        [DevAuth]
        public ActionResult ProfilePage()
        {
            int userId = (int)Session["UserId"];
            UserViewModel vm = new UserViewModel()
            {
                User = db.Users.Find(userId),
                Forms = db.Forms.Where(u => u.UserId == userId).ToList(),
                Tags = db.TagLists.ToList(),
                Subscribes = db.Subscribes.ToList(),
                SavedForms = db.SavedForms.ToList()
            };
            return View(vm);
        }


        [HttpGet]
        public ActionResult ProfileViewPage(int? id)
        {
            if (id != null)
            {
                var checkUser = (int?)Session["UserId"];

                if (checkUser != null && id == checkUser)
                {
                    return RedirectToAction(nameof(ProfilePage));
                }
                else
                {
                    UserViewModel vm = new UserViewModel()
                    {
                        User = db.Users.Find(id),
                        Forms = db.Forms.Where(u => u.UserId == id).ToList(),
                        Tags = db.TagLists.ToList(),
                        Subscribes = db.Subscribes.ToList(),
                        SavedForms = db.SavedForms.Where(u=>u.UserDetailId == id).ToList()
                    };
                    return View("ProfilePage", vm);
                }
            }
            else
            {
                return RedirectToAction("UsersInspectPage", "Home");
            }
        }

        //Users page
        public ActionResult UsersInspectPage()
        {
            DevUsersVm vm = new DevUsersVm()
            {
                UserDetails = db.UserDetails.ToList(),
                Subscribes = db.Subscribes.ToList()
            };

            return View(vm);
        }


        //UserPanel page
        [DevAuth]
        public ActionResult UserPanel()
        {
            var userID = (int)Session["UserId"];
            return View(db.UserDetails.Find(userID));
        }


        [HttpPost]
        public ActionResult UserPanel([Bind(Exclude = "Image")]UserDetail userChanges, string firstname, string lastname, string email, string newPassword, string rePassword, HttpPostedFileBase image, string skills, string checker)
        {

            var user = db.Users.Find((int)Session["UserId"]);

            user.FirstName = firstname;
            user.Lastname = lastname;
            user.UserDetail = userChanges;

            //For user IMG
            if (image != null && İmageControl.CheckImageType(image))
            {
                if (İmageControl.CheckImageSize(image, 8))
                {
                    if (user.UserDetail.Image != null)
                    {
                        İmageControl.DeleteImage(Server.MapPath("~/Public/Images/UsersFolder/ProfilePic"), user.UserDetail.Image);
                    }

                    var imgName = İmageControl.SaveImage(Server.MapPath("~/Public/Images/UsersFolder/ProfilePic"), image);
                    user.UserDetail.Image = imgName;
                }
            }


            //For user Skills
            if (skills != null)
            {
                helperUserMethod.AddRemoveSkills(user, skills, checker);
            }


            //For new password
            if (newPassword != null && newPassword.Length >= 6)
            {
                helperUserMethod.ChangePassword(user, newPassword, rePassword);
            }


            //For change email
            if (user.Email != email)
            {
                helperUserMethod.ChangeEmail(user, email);
            }

            db.SaveChanges();
            return RedirectToAction("UserPanel", "Home");
        }


        //Add & remove like
        public async Task<int> Like(int id)
        {
            Form form = await db.Forms.FirstOrDefaultAsync(c => c.Id == id);
            int userId = (int)Session["UserId"];
            var userIsLiked = await db.FormLikes.AnyAsync(x => x.FormId == form.Id && x.UserId == userId);

            if (!userIsLiked)
            {
                await helperMethods.Like(form.Id, userId);
            }
            else
            {
                await helperMethods.Disslike(form.Id, userId);
            }
            return form.FormLikes.Count();
        }



        //Subscribe
        public async Task<int> SubscribeUser(int? id)
        {
            var userId = (int?)Session["UserId"];

            if (userId != null && id != null)
            {
                var checkUser = db.Subscribes.Any(u => u.FollowerId == userId && u.FollowId == id);
                if (!checkUser)
                {
                    await helperMethods.Follow((int)userId, (int)id);
                }
                else
                {
                    await helperMethods.UnFollow((int)userId, (int)id);
                }

            }
            return await db.Subscribes.Where(f => f.FollowId == id).CountAsync();
        }


        //Save Form
        public async Task<ActionResult> SaveForm(int? id)
        {
            if (id != null)
            {
                var userId = (int)Session["UserId"];
                var checkIfSaved = db.SavedForms.Any(u => u.UserDetailId == userId && u.FormId == id);
                if (!checkIfSaved)
                {
                    SavedForm svForm = new SavedForm()
                    {
                        UserDetailId = userId,
                        FormId = (int)id
                    };
                    db.SavedForms.Add(svForm);
                    await db.SaveChangesAsync();
                }
               
            }

            return RedirectToAction("ProfilePage", "Home");
        }


        //Finding forms for tagname
        public ActionResult FindPost(int? id)
        {
            if(id != null)
            {
                var data = db.TagLists.Find(id).FormId;
                var frm = db.Forms.Find(data);
                return View(frm);
            }
            return RedirectToAction("FormPage","Home");
        }
    }
}