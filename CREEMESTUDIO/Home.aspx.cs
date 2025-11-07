using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace CREEMESTUDIO
{
    public partial class Home : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var offers = new List<dynamic>
            {
                new { ImageUrl = "Images/Flat15.jpg",    Title = "Flat 15% Off",   Subtitle = "Gifts over ₹2000" },
                new { ImageUrl = "Images/Upto30.jpg",    Title = "Up to 30% Off", Subtitle = "Singles & combos" },
                new { ImageUrl = "Images/Buy1get1.jpg",  Title = "Buy 1 Get 1",   Subtitle = "Limited time" },
                new { ImageUrl = "Images/FestivePicks.jpg", Title = "Festive Picks", Subtitle = "Handpicked bestsellers" }
            };

                rpOffers.DataSource = offers;
                rpOffers.DataBind();
            }
        }
    }
}