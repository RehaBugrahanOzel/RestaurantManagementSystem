using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using restaurant2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace restaurant2.Controllers
{
    public class HomeController : Controller
    {
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        SqlConnection con = new SqlConnection();
        List<Menu> menu = new List<Menu>();
        public List<Cart> cart = new List<Cart>();
        List<Customer> customer = new List<Customer>();
        List<OrderInfo> orderInfo = new List<OrderInfo>();
        List<AdminPanel> admin_Panel = new List<AdminPanel>();
        List<OrderAdmin> order_admin = new List<OrderAdmin>();
        private readonly ILogger<HomeController> _logger;

        public IActionResult ClearCart()
        {
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "DELETE FROM cart;";
                com.ExecuteNonQuery();
                con.Close();
                
            }
            catch (Exception ex)
            {
                TempData["msg1"] = ex.Message;
                
               
            }
            Privacy();
            return View("Privacy");
        }

        public IActionResult DeleteOneRow(String rowItemId)
        {
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "DELETE FROM Cart WHERE ItemId="+rowItemId+";";
                com.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                TempData["msg1"] = ex.Message;
            }
            Privacy();
            return View("Privacy");
        }


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            con.ConnectionString = restaurant2.Properties.Resources.ConnectionString;
        }

        public IActionResult Index()
        {
            fetchMenuTable();
            return View(menu);
        }
        public int itemValueCalc()
        {
            return 0;
        }
        public ActionResult userInfo(String name, String lastName, String email, String phone, String address, String payment, String message)
        {
            int totalPrice = 0;
            int paymentId = 0;
            //Calculatin cart total from database
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT TOP (1000) [ItemId],[ItemName],[ItemInfo],[ItemQuantity],[TotalPrice],[ItemPrice]FROM[RestourantManagementSys].[dbo].[Cart]";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    totalPrice += (int)dr["TotalPrice"];
                }
                con.Close();

            }
            catch (Exception ex)
            {
                throw ex;
                TempData["msg1"] = ex.Message;
            }

            //Creating payment info on payment table in database
            try
            {
                DateTime now = new DateTime();
                String Date = now.ToString("F");
                con.Open();
                com.Connection = con;
                com.CommandText = "INSERT INTO Payment(PaymentDate, Amount, PaymentType) VALUES ('" + Date + "','" + totalPrice + "','" + payment+"');";
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
                TempData["msg1"] = ex.Message;
            }

            //Select paymentId According to yapment type
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT PaymentId From Payment where PaymentType = '"+payment+"';";
                dr = com.ExecuteReader();
                dr.Read();
                paymentId = (int)dr["PaymentId"];
                //paymentId = com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
                TempData["msg1"] = ex.Message;
            }

            //inserting data to customer table in data base
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "INSERT INTO Customer(FirstName, LastName, Email, PhoneNo, Address, PaymentId, Message) VALUES ('"+name+"','"+lastName+"','"+email+"','"+phone+"','"+address+"','"+paymentId+"','"+message+"')";
                com.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                throw ex;
                TempData["msg1"] = ex.Message;
            }
            fetchOrderInfo();
            return View(orderInfo);
        }

        public IActionResult decreaseQuantity(int quantity, String rowItemId, int price)
        {
            if (quantity >1)
            {
                quantity--;
            }
            int total = price * quantity;
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "UPDATE Cart SET ItemQuantity = " + quantity.ToString() + " WHERE ItemId = " + rowItemId + "; ";
                com.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                throw ex;
                TempData["msg1"] = ex.Message;
            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "UPDATE Cart SET TotalPrice = " + total.ToString() + " WHERE ItemId = " + rowItemId + "; ";
                com.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                throw ex;
                TempData["msg1"] = ex.Message;
            }
            Privacy();
            return View("Privacy");
        }
        public IActionResult increaseQuantity(int quantity, String rowItemId, int price)
        {
            quantity++;
            int total = price * quantity;
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "UPDATE Cart SET ItemQuantity = " + quantity.ToString() + " WHERE ItemId = " + rowItemId + "; ";
                com.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                throw ex;
                TempData["msg1"] = ex.Message;
            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "UPDATE Cart SET TotalPrice = " + total.ToString() + " WHERE ItemId = " + rowItemId + "; ";
                com.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                throw ex;
                TempData["msg1"] = ex.Message;
            }
            Privacy();
            return View("Privacy");
        }
        public void addToCart(String Image, String FoodName, String About, int Price)
        {

            cart.Add(new Cart() {CartImage = Image, CartFoodName = FoodName, CartAbout = About, CartPrice= Price, CartQuantity=1, CartTotalPrice= Price});

        }

        private void fetchOrderInfo()
        {
            if (orderInfo.Count > 0)
            {
                orderInfo.Clear();
            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT TOP (1000) [ItemId],[ItemName],[ItemInfo],[ItemQuantity],[TotalPrice],[ItemPrice],[ItemImage]FROM[RestourantManagementSys].[dbo].[Cart]";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    orderInfo.Add(new OrderInfo() { OrderCartFoodId = (int)dr["ItemId"], OrderCartImage = dr["ItemImage"].ToString(), OrderCartFoodName = dr["ItemName"].ToString(), OrderCartAbout = dr["ItemInfo"].ToString(), OrderCartPrice = (int)dr["ItemPrice"], OrderCartQuantity = (int)dr["ItemQuantity"], OrderCartTotalPrice = (int)dr["TotalPrice"] });
                }
                con.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT TOP 1  [CustomerId],[FirstName],[LastName],[Email],[PhoneNo],[Address],[PaymentId],[Message] FROM[RestourantManagementSys].[dbo].[Customer] ORDER BY CustomerId DESC";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    orderInfo.Add(new OrderInfo() { OrderCustomerId = (int)dr["CustomerId"],OrderCustomerName = dr["FirstName"].ToString(), OrderCustomerLastName = dr["LastName"].ToString(), OrderCustomerEmail = dr["Email"].ToString(), OrderCustomerPhoneNo = dr["PhoneNo"].ToString(), OrderCustomerAddress = dr["Address"].ToString(), OrderCustomerPaymentId = (int)dr["PaymentId"], OrderCustomerMessage = dr["Message"].ToString() });
                }
                con.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void fetchOrderAdmin()
        {
            if (order_admin.Count > 0)
            {
                order_admin.Clear();
            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT TOP (1000) [OrderId],[CustomerInfo],[OrderInfo],[OrderPrice] FROM[RestourantManagementSys].[dbo].[Order]";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    order_admin.Add(new OrderAdmin() { OrderId = (int)dr["OrderId"], CustomerInfo = dr["CustomerInfo"].ToString(), OrderInfo = dr["OrderInfo"].ToString(), OrderPrice = (int)dr["OrderPrice"] });
                }
                con.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void fetchCartTable()
        {
            if (cart.Count > 0)
            {
                cart.Clear();
            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT TOP (1000) [ItemId],[ItemName],[ItemInfo],[ItemQuantity],[TotalPrice],[ItemPrice],[ItemImage] FROM[RestourantManagementSys].[dbo].[Cart]";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    cart.Add(new Cart() {CartFoodId = (int)dr["ItemId"], CartImage = dr["ItemImage"].ToString(), CartFoodName = dr["ItemName"].ToString(), CartAbout = dr["ItemInfo"].ToString(), CartPrice = (int)dr["ItemPrice"], CartQuantity = (int)dr["ItemQuantity"], CartTotalPrice = (int)dr["TotalPrice"] });
                }
                con.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void fetchCustomerTable()
        {
            if (customer.Count > 0)
            {
                customer.Clear();
            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT TOP (1000) [CustomerId],[FirstName],[LastName],[Email],[PhoneNo],[Address],[PaymentId],[Message] FROM[RestourantManagementSys].[dbo].[Customer]";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    customer.Add(new Customer() { CustomerId = (int)dr["CustomerId"], CustomerName = dr["FirstName"].ToString(), CustomerLastName = dr["LastName"].ToString(), CustomerEmail = dr["Email"].ToString(), CustomerPhoneNo = dr["PhoneNo"].ToString(), CustomerAddress = dr["Address"].ToString(), CustomerPaymentId = (int)dr["PaymentId"], CustomerMessage = dr["Message"].ToString() });
                }
                con.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void fetchMenuTable()
        {
            if (menu.Count >0)
            {
                menu.Clear();
            }

            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT TOP (1000) [FoodId] ,[Price] ,[FoodName] ,[Catagory] ,[About], [Image] FROM [RestourantManagementSys].[dbo].[Menu]";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    menu.Add(new Menu() { FoodId = dr["FoodId"].ToString(), Price = (int)dr["Price"], FoodName = dr["FoodName"].ToString(), Catagory = dr["Catagory"].ToString(), About = dr["About"].ToString(), Image = dr["Image"].ToString()});
                }
                con.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public IActionResult CartPopUp(int CartFoodId, String CartImage, String CartFoodName, String CartFoodInfo, int CartFoodPrice)
        {
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "INSERT INTO [dbo].[Cart](ItemId,ItemName,ItemInfo,ItemQuantity,TotalPrice,ItemPrice,ItemImage) VALUES('" + CartFoodId + "','" + CartFoodName + "','" + CartFoodInfo + "','" + 1 + "','" + CartFoodPrice + "','" + CartFoodPrice + "', '"+CartImage+"')";
                com.ExecuteNonQuery();
                con.Close();
                
                TempData["msg"] = "Succesfull!";
                
            }
            catch (Exception ex)
            {

                TempData["msg"] = "Item already in the cart!";
            }
            return View();

        }
        public IActionResult passwordCheck(String username, String password)
        {
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT TOP (1000) [AdminUsername],[AdminPassword]FROM[RestourantManagementSys].[dbo].[Administrator]";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    if (username.Equals(dr["AdminUsername"].ToString()))
                    {
                        if (password.Equals(dr["AdminPassword"].ToString()))
                        {
                            return View("adminMenu");
                        }
                        else TempData["msg2"] = "Password incorrect!";

                    }
                    else TempData["msg2"] = "Username Incorrect!";
                }
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
                TempData["msg1"] = ex.Message;
            }
            return View("adminPanel");
        }
        public IActionResult adminMenuEdit()
        {
            fetchMenuTable();
            return View(menu);
        }

        public IActionResult addNewMenuItem(String Id, String Image,String Category, String Name, String About, int Price)
        {

            
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "INSERT INTO Menu(FoodId, Price, FoodName, Catagory, About, Image) VALUES ('" + Id + "','" + Price + "','" + Name + "','" + Category + "','" + About + "','" + Image + "');";
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
                TempData["msg1"] = ex.Message;
            }

            adminMenuEdit();
            return View("adminMenuEdit");
        }

        public IActionResult addNewAdmin(String AdminUsername, String AdminPassword)
        {


            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "INSERT INTO Administrator(AdminUsername, AdminPassword) VALUES ('" + AdminUsername + "','" + AdminPassword + "');";
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
                TempData["msg1"] = ex.Message;
            }

            addAndRemoveAdmin();
            return View("addAndRemoveAdmin");
        }

        public IActionResult fillingOrderTable(String CustomerInfo, String OrderInfo, int OrderPrice)
        {

            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "INSERT INTO [Order](CustomerInfo, OrderInfo, OrderPrice) VALUES ('" + CustomerInfo + "','" + OrderInfo + "','" + OrderPrice + "');";
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
                TempData["msg1"] = ex.Message;
            }
            fillOrderTable();
            return View("fillOrderTable");
        }
        public IActionResult fillOrderTable()
        {
            fetchOrderInfo();
            return View(orderInfo);
        }

        public IActionResult listCustomers()
        {
            fetchCustomerTable();
            return View(customer);
        }

        public void fetchAdministratorTable()
        {
            if (admin_Panel.Count > 0)
            {
                admin_Panel.Clear();
            }

            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT TOP (1000) [AdminId] ,[AdminUsername] ,[AdminPassword] FROM [RestourantManagementSys].[dbo].[Administrator]";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    admin_Panel.Add(new AdminPanel() { AdminId = (int)dr["AdminId"], AdminUsername = dr["AdminUsername"].ToString(), AdminPassword = dr["AdminPassword"].ToString()});
                }
                con.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public IActionResult removeAdmin(int AdminId)
        {
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "DELETE FROM Administrator WHERE AdminId=" + AdminId + ";";
                com.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                TempData["msg1"] = ex.Message;
            }
            addAndRemoveAdmin();
            return View("addAndRemoveAdmin");
        }

        public IActionResult addAndRemoveAdmin()
        {


            fetchAdministratorTable();
            return View(admin_Panel);
        }

        public IActionResult removeFromMenu(int CartFoodId)
        {
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "DELETE FROM Menu WHERE FoodId=" + CartFoodId + ";";
                com.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                TempData["msg1"] = ex.Message;
            }
            adminMenuEdit();
            return View("adminMenuEdit");
        }

        public IActionResult showOrders()
        {
            fetchOrderAdmin();
            return View(order_admin);
        }
        public IActionResult adminMenu()
        {
            return View();
        }
        public IActionResult adminPanel(String username, String password)
        {
            return View();
        }
        public IActionResult Privacy()
        {

            fetchCartTable();
            return View(cart);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
