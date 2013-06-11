using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DavidLoginSystem.Common
{
    public class LoginHelper
    {
        public static bool IsLogin()
        {
            try
            {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}