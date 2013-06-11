using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DavidLoginSystem.Models;
using CommonUtil;
//using David.Commons.Helper;

namespace DavidLoginSystem.Controllers
{
    public class UserController : BaseController
    {
        private LoginSystemEntities db;

        #region View Region

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult Add(string id)
        {
            User newUser = new User();
            return View("Edit", newUser);
        }

        public ActionResult Edit(string id)
        {
            using (db = new LoginSystemEntities())
            {
                int queryId = id.ToInt();
                User viewUser = new User();
                if (queryId == 0)
                {
                    viewUser = new User();
                }
                else
                {
                    if (queryId > 0)
                    {
                        viewUser = db.Users.FirstOrDefault(n => n.Id == queryId);
                        return View("Edit", viewUser);
                    }
                }

                return View("Edit", viewUser);
            }
        }

        public ActionResult Details()
        {
            using (db = new LoginSystemEntities())
            {
                int id = RequestUtil.Query("id").ToInt();
                User detailUser = db.Users.FirstOrDefault(n => n.Id == id);
                if (detailUser == null)
                    detailUser = new User();

                return View("Details", detailUser);
            }
        }

        #endregion

        #region JsonResult Region

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserList()
        {
            int pageStart = RequestUtil.Query("iDisplayStart").ToInt();
            int pageSize = RequestUtil.Query("iDisplayLength").ToInt();
            int sortColumn = RequestUtil.Query("iSortCol_0").ToInt();
            bool isDesc = RequestUtil.Query("sSortDir_0").ToString() == "desc";
            string keyword = RequestUtil.Query("searchBox").ToString();

            using (LoginSystemEntities db = new LoginSystemEntities())
            {
                IList<User> userLs = db.Users.Where(n => n.Status > 0).OrderByDescending(n => n.LastChanged).ToList<User>();
                int totalCount = userLs.Count;

                switch (sortColumn)
                {
                    case 0:
                        {
                            if (isDesc)
                                userLs = userLs.OrderByDescending(n => n.Id).ToList<User>();
                            else
                                userLs = userLs.OrderBy(n => n.Id).ToList<User>();
                        }; break;
                    case 1:
                        {
                            if (isDesc)
                                userLs = userLs.OrderByDescending(n => n.UserName).ToList<User>();
                            else
                                userLs = userLs.OrderBy(n => n.UserName).ToList<User>();
                        }; break;
                    case 2:
                        {
                            if (isDesc)
                                userLs = userLs.OrderByDescending(n => n.Email).ToList<User>();
                            else
                                userLs = userLs.OrderBy(n => n.Email).ToList<User>();
                        }; break;
                    case 3:
                        {
                            if (isDesc)
                                userLs = userLs.OrderByDescending(n => n.LastChanged).ToList<User>();
                            else
                                userLs = userLs.OrderBy(n => n.LastChanged).ToList<User>();
                        }; break;
                    default:
                        userLs = userLs.OrderByDescending(n => n.Id).ToList<User>(); break;
                }

                userLs = userLs.Where(n => n.Id == keyword.ToInt() || n.UserName.Contains(keyword)).ToList<User>();

                userLs = userLs.Skip(pageStart).Take(pageSize).ToList<User>();
                List<string> optionsLs = new List<string>(){
                    "<a href=\"/User/Edit?id={0}\" class=\"ml8\">Edit</a>",
                    "<a href=\"/User/Details?id={0}\" class=\"ml8\">Details</a>",
                    "<a href=\"javascript:deleteUser({0})\" class=\"ml8\">Delete</a>"
                };

                var result = userLs.Select(n => new object[]
                {
                    n.Id,
                    n.UserName,
                    n.Email,
                    n.LastChanged.Value.ToShortDateString(),
                    string.Format(string.Join("", optionsLs), n.Id)
                });

                return Json(new { aaData = result, iTotalDisplayRecords = totalCount, iTotalRecords = totalCount }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 编辑User
        /// </summary>
        /// <param name="formUser"></param>
        /// <returns></returns>
        public ActionResult EditUser(User formUser)
        {
            using (db = new LoginSystemEntities())
            {
                try
                {
                    if (formUser.Id > 0)
                    {
                        User editUser = db.Users.FirstOrDefault(n => n.Status > 0 && n.Id == formUser.Id);
                        if (editUser != null)
                        {
                            editUser.UserName = formUser.UserName;
                            editUser.Password = formUser.Password;
                            editUser.Email = formUser.Email;
                            editUser.Status = 1;
                            editUser.LastChanged = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        User newUser = new User();
                        newUser.UserName = formUser.UserName;
                        newUser.Password = formUser.Password;
                        newUser.Email = formUser.Email;
                        newUser.Status = 1;
                        newUser.CreateTime = DateTime.Now;
                        newUser.LastChanged = DateTime.Now;
                        db.Users.AddObject(newUser);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    ViewData["error"] = "更新失败!" + ex.Message;
                    return View("Edit", formUser);
                }

                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteUser()
        {
            int id = RequestUtil.Form("id").ToInt();
            bool isSuccess = false;
            string errorMsg = string.Empty;
            using (db = new LoginSystemEntities())
            {
                try
                {
                    User delUser = db.Users.FirstOrDefault(n => n.Id == id && n.Status > 0);
                    if (delUser != null)
                    {
                        delUser.Status = -1;
                        db.SaveChanges();
                    }
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    isSuccess = false;
                }
            }

            return Json(new { flag = isSuccess == true ? 1 : 0, msg = errorMsg }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
