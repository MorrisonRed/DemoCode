using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;

namespace democode.mvc.Controllers
{
    public class ProjectsController : Controllers.BaseController
    {
        List<democode.mvc.Models.ProjectModels> projects = new List<democode.mvc.Models.ProjectModels>();

        private democode.mvc.Models.ProjectModels convertToProjectModels(DataRow dr)
        {
            democode.mvc.Models.ProjectModels p = new democode.mvc.Models.ProjectModels();
            try
            {
                if (dr["pid"] != System.DBNull.Value) { p.PID = new Guid(dr["pid"].ToString()); }
                if (dr["ProjIcon"] != System.DBNull.Value) { p.Icon = (String)dr["ProjIcon"]; }
                if (dr["ProjCode"] != System.DBNull.Value) { p.Code = (String)dr["ProjCode"]; }
                if (dr["ProjName"] != System.DBNull.Value) { p.Name = (String)dr["ProjName"]; }
                if (dr["ProjDesc"] != System.DBNull.Value) { p.Description = (String)dr["ProjDesc"]; }
                if (dr["ProjEstStartDate"] != System.DBNull.Value) { p.EstimatedStartDate = (DateTime)dr["ProjEstStartDate"]; }
                if (dr["ProjEstEndDate"] != System.DBNull.Value) { p.EstimatedEndDate = (DateTime)dr["ProjEstEndDate"]; }
                if (dr["ProjActStartDate"] != System.DBNull.Value) { p.EstimatedStartDate = (DateTime)dr["ProjActStartDate"]; }
                if (dr["ProjActEndDate"] != System.DBNull.Value) { p.ActualEndDate = (DateTime)dr["ProjActEndDate"]; }
                if (dr["ProjFolder"] != System.DBNull.Value) { p.Folder = (String)dr["ProjFolder"]; }
                if (dr["ProjCaption"] != System.DBNull.Value) { p.Caption = (String)dr["ProjCaption"]; }
                if (dr["ProjURL"] != System.DBNull.Value) { p.URL = (String)dr["ProjURL"]; }
                if (dr["ProjOrg"] != System.DBNull.Value) { p.Organization = (String)dr["ProjOrg"]; }

                return p;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private democode.mvc.Models.ProjectModels convertToProjectModels(ProjectMgmt.Project data)
        {
            democode.mvc.Models.ProjectModels p = new democode.mvc.Models.ProjectModels();
            try
            {
                p.PID = data.PID;
                p.Icon = data.Icon;
                p.Code = data.Code;
                p.Name = data.Name;
                p.Description = data.Description;
                p.EstimatedStartDate = data.EstimatedStartDate;
                p.EstimatedEndDate = data.EstimatedEndDate;
                p.ActualStartDate = data.ActualStartDate;
                p.ActualEndDate = data.ActualEndDate;
                p.Folder = data.Folder;
                p.Caption = data.Caption;
                p.URL = data.URL;
                p.Organization = data.Organization;

                return p;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private ProjectMgmt.Project convertFromProject(democode.mvc.Models.ProjectModels data)
        {
            ProjectMgmt.Project p = new ProjectMgmt.Project();
            try
            {
                p.PID = data.PID;
                p.Icon = data.Icon;
                p.Code = data.Code;
                p.Name = data.Name;
                p.Description = data.Description;
                p.EstimatedStartDate = data.EstimatedStartDate;
                p.EstimatedEndDate = data.EstimatedEndDate;
                p.ActualStartDate = data.ActualStartDate;
                p.ActualEndDate = data.ActualEndDate;
                p.Folder = data.Folder;
                p.Caption = data.Caption;
                p.URL = data.URL;
                p.Organization = data.Organization;

                return p;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET: Projects
        [Route("projects")]
        [Route("projects/{taxonomy?}")]
        public ActionResult Index(string taxonomy)
        {
            //get projects from system 
            //if no taxonomy is provided then return all projects
            List<ProjectMgmt.Project> pros;
            if (string.IsNullOrEmpty(taxonomy))
            {
                projects = new List<democode.mvc.Models.ProjectModels>();
                pros = ProjectMgmt.Project.ListFor(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(),
                    "ProjEstEndDate", ProjectMgmt.SortDirection.Descending);
            }
            else
            {
                projects = new List<democode.mvc.Models.ProjectModels>();
                pros = ProjectMgmt.Project.ListFor(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(),
                    taxonomy, "ProjEstEndDate", ProjectMgmt.SortDirection.Descending);
            }


            if (pros == null || pros.Count == 0) return View(projects);

            foreach (ProjectMgmt.Project p in pros)
            //foreach (DataRow dr in dt.Rows)
            {
                var ev = convertToProjectModels(p);
                if (ev != null) projects.Add(ev);
            }

            return View(projects);
        }

        // GET: Specific Project
        [Route("projects/project/{code}")]
        public ActionResult Project(string code)
        {
            ProjectMgmt.Project proj = new ProjectMgmt.Project(ConfigurationManager.ConnectionStrings["SystemDS"].ToString(), code);

            democode.mvc.Models.ProjectModels model = convertToProjectModels(proj);
            model.ProjectImages = new List<String>();
            String projectImagesFolder = model.Folder;

            //if no folder is listed then just return view
            if (!string.IsNullOrEmpty(projectImagesFolder))
            {
                if (!projectImagesFolder.EndsWith("/")) projectImagesFolder += "/";
                projectImagesFolder += "images/";


                //check if folder exist if true then get all images in folder
                if (Directory.Exists(Server.MapPath("~" + projectImagesFolder)))
                {
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Server.MapPath("~" + projectImagesFolder));
                    System.IO.FileInfo[] diar1 = di.GetFiles();
                    //list the names of all files in the specified directory
                    foreach (System.IO.FileInfo dra in diar1)
                    {
                        if (dra.Extension.ToLower() == ".jpg" || dra.Extension.ToLower() == ".png" || dra.Extension.ToLower() == ".gif")
                        {
                            model.ProjectImages.Add(String.Format("{0}{1}", projectImagesFolder, dra.Name));
                        }
                    }
                }              
            }

            return View(model);
        }
    }
}