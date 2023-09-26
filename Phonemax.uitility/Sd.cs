using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonemax.uitility
{
    public static class Sd
    {
        // roles
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee user";
        public const string Role_Company = "Company user";
        public const string Role_Individual = "Individual user";
        //session
        public const string Ss_CartSessionCount = "Cart count session";
        //order status
        public const string orderstatuspending = "pending";
        public const string orderstatusApproved = "Approved";
        public const string orderstatusInprogress = "Processing";
        public const string orderstatusShipped = "Shipped";
        public const string orderstatusCancelled = "Cancelled";
        public const string orderstatusRefunded = "Refunded";

        //paymentstatus

        public const string paymentstatusPending = "pending";
        public const string paymentstatusApproved = "Approved";
        public const string paymentstatusdelayPayment = "Paymentstatusdelay";
        public const string paymentstatusRejected = "Rejected";


        public static double GetpricebasedonQuantity(double quantity, double price, double price50, double price100)
        {
            if (quantity < 50)
                return price;
            else if (quantity < 100)
                return price50;
            else return price100;

        }
        public static string ConvertToRawHtml(string source)
        {
            char[] array = new char[source.Length];
            int arrayindex = 0;
            bool inside = false;
            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }

                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayindex] = let;
                    arrayindex++;
                }
            }
            return new string(array, 0, arrayindex);
        }

    }
}
