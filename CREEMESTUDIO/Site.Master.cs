using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
namespace CREEMESTUDIO
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var role = Session["Role"] as string;
            var loggedIn = Session["UserId"] != null;

            phAnon.Visible = !loggedIn;
            phUser.Visible = loggedIn;
            phAdmin.Visible = (loggedIn && string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase));
        }
    }
}