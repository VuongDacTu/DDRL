﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Admin.Models
{
    public class IsDelete
    {
        private readonly IHttpContextAccessor _context;
        public IsDelete(IHttpContextAccessor context)
        {
            _context = context;
        }
        public bool An()
        {
            
            var admin = JsonConvert.DeserializeObject<AccountAdmin>(_context.HttpContext?.Session.GetString("AdminLogin") ?? "");
            if (admin?.RoleId != 1 && admin != null)
            {
                _context.HttpContext?.Response.Redirect("/Admin/Status/Index");
            }
            return true;
        }
        public bool Hien()
        {
            return false;
        }
    }
}
