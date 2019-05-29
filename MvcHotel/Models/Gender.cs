using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcHotel.Models
{
    public static class Gender
    {
        public static MvcHtmlString GenderText(this HtmlHelper htmlHelper, bool? sex)
        {
            string text = "保密";
            if (!sex.HasValue)
            {
                text = (bool)sex ? "男" : "女";
            }
            return new MvcHtmlString(text);
        }

    }
}