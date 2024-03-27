using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Assignment_3
{
    internal class Assign3
    {
        private static DataSet dataSet = new DataSet();
        static void Main(string[] args)
        { 
            DataTable order = new DataTable("Order");
            order.Columns.Add("OrderID", typeof(int));
            order.Columns.Add("ProductName", typeof(string));
            order.Columns.Add("ProductCode", typeof(string));
            order.Columns.Add("ProductSize", typeof(string));
            order.Columns.Add("CustomerAddress", typeof(string));
            order.Columns.Add("CustomerContact", typeof(string));
            order.Columns.Add("ProductQuantity", typeof(int));
            order.Columns.Add("Price", typeof(int));
            order.Columns.Add("CustomerName", typeof(string));
            dataSet.Tables.Add(order);
            order.PrimaryKey = new DataColumn[] { order.Columns["OrderID"] };

            DataTable customer = new DataTable("Customer");
            customer.Columns.Add("CustomerID", typeof(string));
            customer.Columns.Add("CustomerName", typeof(string));
            customer.Columns.Add("CustomerAddress", typeof(string));
            customer.Columns.Add("CustomerContact", typeof(string));
            customer.PrimaryKey = new DataColumn[] { customer.Columns["CustomerID"] };
            dataSet.Tables.Add(customer);

            DataTable product = new DataTable("Product");
            product.Columns.Add("ProductCode", typeof(string));
            product.Columns.Add("ProductName", typeof(string));
            product.Columns.Add("ProductPrice", typeof(int));
            product.Columns.Add("ProductPicture", typeof(string));
            product.PrimaryKey = new DataColumn[] { product.Columns["ProductCode"] };
            dataSet.Tables.Add(product);
            bool check = false;
            while (!check)
            {
                Console.WriteLine("\t\t------------------------------Welcome to Queen's Clothing Shop-------------------------------");
                Console.WriteLine("1. Retrieve Order");
                Console.WriteLine("2. Modify Order Details");
                Console.WriteLine("3. Add New Order");
                Console.WriteLine("4. Remove Order");
                Console.WriteLine("5. Update Database");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice: ");

                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            RetrieveOrder(order);
                            break;
                        case 2:
                            ModifyOrderDetails(dataSet);
                            break;
                        case 3:
                            AddNewOrder(dataSet);
                            break;
                        case 4:
                            RemoveOrder(dataSet);
                            break;
                        case 5:
                            UpdateDatabase(dataSet);
                           break;
                        case 6:
                            Console.WriteLine("Exiting Program! GoodBye[-_-]");
                            return;
                       default:
                            Console.WriteLine("Invalid choice! Please enter a number between 1 and 6.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter a number.");
                }
            }
        }
        static void RetrieveOrder(DataTable order)
        {
            string constring = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Newqueensdb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
            Console.WriteLine("Enter the OrderID:");
            int orderId;
            int.TryParse(Console.ReadLine(), out orderId);
            string query = "SELECT * FROM [Order] WHERE OrderID = @OrderID";

            using (SqlConnection sconnect = new SqlConnection(constring))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                SqlCommand scommand = new SqlCommand(query, sconnect);
                dataAdapter.SelectCommand = scommand;
                dataAdapter.SelectCommand.Parameters.AddWithValue("@OrderID", orderId);
                sconnect.Open();
                dataAdapter.Fill(order);
                foreach (DataRow row in order.Rows)
                {
                    Console.WriteLine($"OrderID: {row["OrderID"]}");
                    Console.WriteLine($"ProductName: {row["ProductName"]}");
                    Console.WriteLine($"ProductCode: {row["ProductCode"]}");
                    Console.WriteLine($"ProductSize: {row["ProductSize"]}");
                    Console.WriteLine($"ProductQuantity: {row["ProductQuantity"]}");
                    Console.WriteLine($"Price: {row["Price"]}");
                    Console.WriteLine($"CustomerName: {row["CustomerName"]}");
                    Console.WriteLine($"CustomerAddress: {row["CustomerAddress"]}");
                    Console.WriteLine($"CustomerContact: {row["CustomerContact"]}");
                    Console.WriteLine();
                }

            }
        }
        static void ModifyOrderDetails(DataSet ds)
        {
            Console.Write("Enter OrderID to modify: ");
            int oid = int.Parse(Console.ReadLine());
            DataTable otable = dataSet.Tables["Order"];
            DataRow orow = otable.Rows.Find(oid);
            if (orow != null)
            {
                Console.WriteLine("Current order details:");
                Console.WriteLine($"ProductName: {orow["ProductName"]}, ProductQuantity: {orow["ProductQuantity"]}");
                Console.Write("Enter new quantity for product: ");
                int newQuantity = int.Parse(Console.ReadLine());
                orow["ProductQuantity"] = newQuantity;
                Console.WriteLine("Order details updated successfully!");
                Console.WriteLine("Modified Order details:");
                Console.WriteLine($"ProductName: {orow["ProductName"]}, ProductQuantity: {orow["ProductQuantity"]}");
            }
            else
            {
                Console.WriteLine("Order not found!");
            }
        }
        static void AddNewOrder(DataSet ds)
        {
            DataTable ot = ds.Tables["Order"];
            Console.WriteLine("Enter details for the new order:");
            Console.Write("Product Name: ");
            string pname = Console.ReadLine();
            Console.Write("Product Code: ");
            string pcode = Console.ReadLine();
            Console.Write("Product Size: ");
            string psize = Console.ReadLine();
            Console.Write("Customer Address: ");
            string cust_add = Console.ReadLine();
            Console.Write("Customer Contact: ");
            string cust_contact = Console.ReadLine();
            Console.Write("Product Quantity: ");
            int pquantity;
            while (!int.TryParse(Console.ReadLine(), out pquantity) || pquantity < 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid positive integer for Product Quantity:");
            }
            Console.Write("Price: ");
            int price;
            while (!int.TryParse(Console.ReadLine(), out price) || price < 0)
            {
                Console.WriteLine("Product Price cannot be negative! Please enter a positive integer for Product Price:");
            }
            Console.Write("Customer Name: ");
            string customerName = Console.ReadLine();
            Console.Write("Order ID: ");
            int orderId;
            while (!int.TryParse(Console.ReadLine(), out orderId) || orderId < 0)
            {
                Console.WriteLine("Order ID cannot be negative!Please enter a positive integer for Order ID:");
            }
            DataRow o_row = ot.NewRow();
            o_row["ProductName"] = pname;
            o_row["ProductCode"] = pcode;
            o_row["ProductSize"] = psize;
            o_row["CustomerAddress"] = cust_add;
            o_row["CustomerContact"] = cust_contact;
            o_row["ProductQuantity"] = pquantity;
            o_row["Price"] = price;
            o_row["CustomerName"] = customerName;
            o_row["OrderID"] = orderId;
            ot.Rows.Add(o_row);
            Console.WriteLine("New order added successfully!");
        }

        static void RemoveOrder(DataSet ds)
        {
            DataTable otable = ds.Tables["Order"];
            Console.WriteLine("Enter the OrderID of the order you want to remove:");
            int remove_oid;
            while (!int.TryParse(Console.ReadLine(), out remove_oid))
            {
                Console.WriteLine("Invalid input. Please enter a valid OrderID:");
            }
            DataRow del_row = otable.Rows.Find(remove_oid);
            if (del_row != null)
            {
                otable.Rows.Remove(del_row);
                Console.WriteLine($"Order with OrderID {remove_oid} removed successfully!");
            }
            else
            {
                Console.WriteLine($"Order with OrderID {remove_oid} not found!");
            }
        }
        static void UpdateDatabase(DataSet ds)
        {
            string constring = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Newqueensdb;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
            using (SqlConnection sconnect = new SqlConnection(constring))
            {
                sconnect.Open();
                DataTable ot = ds.Tables["Order"];
                string query = "SELECT * FROM [Order]";
                SqlDataAdapter order_data_adap = new SqlDataAdapter();
                SqlCommand scommand = new SqlCommand(query, sconnect);
                order_data_adap.SelectCommand = scommand;
                order_data_adap.InsertCommand = new SqlCommand("INSERT INTO [Order] (ProductName, ProductCode, ProductSize, CustomerAddress, CustomerContact, ProductQuantity, Price, CustomerName, OrderID) VALUES (@ProductName, @ProductCode, @ProductSize, @CustomerAddress, @CustomerContact, @ProductQuantity, @Price, @CustomerName, @OrderID)", sconnect);
                order_data_adap.InsertCommand.Parameters.Add("@OrderID", SqlDbType.Int, 0, "OrderID");
                order_data_adap.InsertCommand.Parameters.Add("@ProductName", SqlDbType.NVarChar, 50, "ProductName");
                order_data_adap.InsertCommand.Parameters.Add("@ProductCode", SqlDbType.NVarChar, 50, "ProductCode");
                order_data_adap.InsertCommand.Parameters.Add("@ProductSize", SqlDbType.NVarChar, 50, "ProductSize");
                order_data_adap.InsertCommand.Parameters.Add("@CustomerAddress", SqlDbType.NVarChar, 50, "CustomerAddress");
                order_data_adap.InsertCommand.Parameters.Add("@CustomerContact", SqlDbType.NVarChar, 50, "CustomerContact");
                order_data_adap.InsertCommand.Parameters.Add("@ProductQuantity", SqlDbType.Int, 0, "ProductQuantity");
                order_data_adap.InsertCommand.Parameters.Add("@Price", SqlDbType.Int, 0, "Price");
                order_data_adap.InsertCommand.Parameters.Add("@CustomerName", SqlDbType.NVarChar, 50, "CustomerName");

                order_data_adap.UpdateCommand = new SqlCommand("UPDATE [Order] SET ProductName = @ProductName, ProductCode = @ProductCode, ProductSize = @ProductSize, CustomerAddress = @CustomerAddress, CustomerContact = @CustomerContact, ProductQuantity = @ProductQuantity, Price = @Price, CustomerName = @CustomerName WHERE OrderID = @OrderID", sconnect);
                order_data_adap.UpdateCommand.Parameters.Add("@OrderID", SqlDbType.Int, 0, "OrderID");
                order_data_adap.UpdateCommand.Parameters.Add("@ProductName", SqlDbType.NVarChar, 50, "ProductName");
                order_data_adap.UpdateCommand.Parameters.Add("@ProductCode", SqlDbType.NVarChar, 50, "ProductCode");
                order_data_adap.UpdateCommand.Parameters.Add("@ProductSize", SqlDbType.NVarChar, 50, "ProductSize");
                order_data_adap.UpdateCommand.Parameters.Add("@CustomerAddress", SqlDbType.NVarChar, 50, "CustomerAddress");
                order_data_adap.UpdateCommand.Parameters.Add("@CustomerContact", SqlDbType.NVarChar, 50, "CustomerContact");
                order_data_adap.UpdateCommand.Parameters.Add("@ProductQuantity", SqlDbType.Int, 0, "ProductQuantity");
                order_data_adap.UpdateCommand.Parameters.Add("@Price", SqlDbType.Int, 0, "Price");
                order_data_adap.UpdateCommand.Parameters.Add("@CustomerName", SqlDbType.NVarChar, 50, "CustomerName");

                order_data_adap.DeleteCommand = new SqlCommand("DELETE FROM [Order] WHERE OrderID = @OrderID", sconnect);
                order_data_adap.DeleteCommand.Parameters.Add("@OrderID", SqlDbType.Int, 0, "OrderID");

                order_data_adap.Update(ot);
                sconnect.Close();
                Console.WriteLine("Changes synchronized with the database successfully.");
            }
        }

    }
}