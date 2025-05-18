using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using ZooManager.Models;

namespace ZooManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();
            //  string connectionString = "Data Source=AlienPC;Initial Catalog=ZooDB;Persist Security Info=True;User ID=sa;Password=SQLDaMicrosoft;Pooling=False;Encrypt=True;Trust Server Certificate=True";
            //  sqlConnection = new SqlConnection(connectionString);
            ShowZoos();
            ShowAnimals();
            //Register();

        }

        private void ShowZoos()
        {
            string connectionString = "Data Source=AlienPC;Initial Catalog=ZooDB;Persist Security Info=True;User ID=sa;Password=SQLDaMicrosoft;Pooling=False;Encrypt=True;Trust Server Certificate=True";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM Zoo";
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                    DataTable zooTable = new DataTable();
                    sqlDataAdapter.Fill(zooTable);

                    // Definindo o DataContext para o ListView
                    listZoos.DisplayMemberPath = "Location";
                    listZoos.SelectedValuePath = "Id";
                    listZoos.ItemsSource = zooTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
            }
        }
        private void ShowAnimals()
        {
            string connectionString = "Data Source=AlienPC;Initial Catalog=ZooDB;Persist Security Info=True;User ID=sa;Password=SQLDaMicrosoft;Pooling=False;Encrypt=True;Trust Server Certificate=True";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM Animal";
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                    DataTable animalTable = new DataTable();
                    sqlDataAdapter.Fill(animalTable);

                    // Definindo o DataContext para o ListView
                    listAnimals.DisplayMemberPath = "Name";
                    listAnimals.SelectedValuePath = "Id";
                    listAnimals.ItemsSource = animalTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
            }
        }
        private void ShowAnimalTexBox()
        {
           
            if (listAnimals.SelectedValue == null)
            {
                return;
            }
            else
            {
                txtBoxZoo.Text = "";
                txtBoxZoo.Text = (listAnimals.SelectedItem as DataRowView)?["Name"].ToString(); 
            }
                


        }
        private void ShowAssociatedAnimals()
        {
            if (listZoos.SelectedValue == null) return;
            string connectionString = "Data Source=AlienPC;Initial Catalog=ZooDB;Persist Security Info=True;User ID=sa;Password=SQLDaMicrosoft;Pooling=False;Encrypt=True;Trust Server Certificate=True";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM Animal a INNER " +
                        "JOIN ZooAnimal za ON a.Id = za.AnimalId " +
                        "WHERE za.ZooId = @ZooId";

                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);



                    command.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

                    DataTable animalTable = new DataTable();
                    sqlDataAdapter.Fill(animalTable);

                    // Definindo o DataContext para o ListView
                    listAssociatedAnimals.DisplayMemberPath = "Name";
                    listAssociatedAnimals.SelectedValuePath = "Id";
                    listAssociatedAnimals.ItemsSource = animalTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                    txtBoxZoo.Text = "";
                    txtBoxZoo.Text = (listZoos.SelectedItem as DataRowView)?["Location"].ToString();
                }
            }
        }
        //object sender, RoutedEventArgs e


        private void listZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // sender is lMessageBox.Show(listZoos.SelectedValue.ToString())          
            if (listZoos.SelectedValue != null)
            {
                ShowAssociatedAnimals();
     
            }

        }
        private void listAnimal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(listAnimals.SelectedValue != null)
            {
                ShowAnimalTexBox();
            }
        }


        private void DeleteZoo_click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=AlienPC;Initial Catalog=ZooDB;Persist Security Info=True;User ID=sa;Password=SQLDaMicrosoft;Pooling=False;Encrypt=True;Trust Server Certificate=True";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "DELETE FROM Zoo WHERE Id = @ZooId";
                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    command.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Zoo deleted sucefully");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                    ShowZoos();
                }
            }
        }

        private void addZoo_click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=AlienPC;Initial Catalog=ZooDB;Persist Security Info=True;User ID=sa;Password=SQLDaMicrosoft;Pooling=False;Encrypt=True;Trust Server Certificate=True";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    bool exists = listZoos.Items.Cast<DataRowView>()
                    .Any(row => row["Location"].ToString().Equals(txtBoxZoo.Text, StringComparison.OrdinalIgnoreCase));

                    if (string.IsNullOrEmpty(txtBoxZoo.Text) || exists)
                    {
                        throw new Exception("Zoologico vazio ou repetido");
                    }
                    else
                    {
                        sqlConnection.Open();
                        string query = "INSERT INTO Zoo(Location) VALUES(@location)";
                        string zooName = txtBoxZoo.Text;
                        zooName = zooName.Trim();
                        zooName = char.ToUpper(zooName[0]) + zooName.Substring(1).ToLower();
                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@location", zooName);
                        sqlCommand.ExecuteNonQuery();
                        MessageBox.Show($"Added the zoo: {zooName}");
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
                finally
                {
                    sqlConnection.Close();

                    txtBoxZoo.Text = "";
                    ShowZoos();
                }

            }
        }

        private void updateZoo_click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=AlienPC;Initial Catalog=ZooDB;Persist Security Info=True;User ID=sa;Password=SQLDaMicrosoft;Pooling=False;Encrypt=True;Trust Server Certificate=True";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    bool exists = listZoos.Items.Cast<DataRowView>()
                    .Any(row => row["Location"].ToString().Equals(txtBoxZoo.Text, StringComparison.OrdinalIgnoreCase));

                    if (string.IsNullOrEmpty(txtBoxZoo.Text) || exists)
                    {
                        throw new Exception("Zoologico vazio ou repetido");
                    }
                    else
                    {
                        //novoValor
                        string newZooValue = txtBoxZoo.Text.Trim();
                        newZooValue = char.ToUpper(newZooValue[0]) + newZooValue.Substring(1).ToLower();

                        sqlConnection.Open();
                        string query = "UPDATE Zoo SET Location = @novoValor WHERE Id = @idCorrespondente";
                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@novoValor", newZooValue);
                        sqlCommand.Parameters.AddWithValue("@idCorrespondente", listZoos.SelectedValue);
                        sqlCommand.ExecuteNonQuery();
                        MessageBox.Show($"UPDATED o zoologico {txtBoxZoo.Text}");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    sqlConnection.Close();

                    txtBoxZoo.Text = "";
                    ShowZoos();
                }
            }
        }
        private void addAnimalZoo_click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=AlienPC;Initial Catalog=ZooDB;Persist Security Info=True;User ID=sa;Password=SQLDaMicrosoft;Pooling=False;Encrypt=True;Trust Server Certificate=True";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    //getting the respected id of animal and zoo so that i can
                    //add to the table ZooAnimal and afterwards be show
                    var selectedZooId = listZoos.SelectedValue;
                    var selectedAnimalId = listAnimals.SelectedValue;

                    bool exists = listAssociatedAnimals.Items.Cast<DataRowView>()
                    .Any(row => row["AnimalId"].ToString().Equals(listAnimals.SelectedValue?.ToString(), StringComparison.OrdinalIgnoreCase));

                    if (exists)
                    {
                        MessageBox.Show("Cant add the same animal");
                        return;
                    }
                    string sqlQuery = "INSERT INTO ZooAnimal VALUES (@ZooId, @AnimalId)";

                    // MessageBox.Show($"ZooId: {selectedZooId}, AnimalId: {selectedAnimalId}");    
                    SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@ZooId", selectedZooId);
                    sqlCommand.Parameters.AddWithValue("@AnimalId", selectedAnimalId);
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show($"Added {(listAnimals.SelectedItem as DataRowView)?["Name"]} to the zoo {(listZoos.SelectedItem as DataRowView)?["Location"]}");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                    ShowAssociatedAnimals();
                }
            }
        }
        private void removeAnimalZoo_click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=AlienPC;Initial Catalog=ZooDB;Persist Security Info=True;User ID=sa;Password=SQLDaMicrosoft;Pooling=False;Encrypt=True;Trust Server Certificate=True";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "DELETE FROM ZooAnimal where AnimalId = @IdAnimal AND ZooId = @IdZoo;";

                    //variables 
                    var idZoo = listZoos.SelectedValue;
                    var idAnimal = listAnimals.SelectedValue;

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@IdAnimal", idAnimal);
                    sqlCommand.Parameters.AddWithValue("@IdZoo", idZoo);
                    sqlCommand.ExecuteNonQuery();

                    MessageBox.Show($"Removed {(listAnimals.SelectedItem as DataRowView)?["Name"]} from the zoo {(listZoos.SelectedItem as DataRowView)?["Location"]}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception: " + ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                    ShowAssociatedAnimals();
                }
            }


        }
        private void addAnimal_click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=AlienPC;Initial Catalog=ZooDB;Persist Security Info=True;User ID=sa;Password=SQLDaMicrosoft;Pooling=False;Encrypt=True;Trust Server Certificate=True";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    string animalName = txtBoxZoo.Text;
                    if (string.IsNullOrEmpty(animalName))
                    {
                        MessageBox.Show("Write something before adding the animal");
                    }

                    animalName = animalName.Trim();
                    animalName = char.ToUpper(animalName[0]) + animalName.Substring(1).ToLower();

                    string query = "INSERT INTO Animal VALUES(@animalName);";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@animalName", animalName);
                    sqlCommand.ExecuteNonQuery();

                    Console.WriteLine($"Added the animal: {animalName}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception:" + ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                    ShowAnimals();
                    txtBoxZoo.Text = "";

                }
            }
        }

        private void deleteAnimal_click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=AlienPC;Initial Catalog=ZooDB;Persist Security Info=True;User ID=sa;Password=SQLDaMicrosoft;Pooling=False;Encrypt=True;Trust Server Certificate=True";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    string query = "DELETE FROM Animal WHERE Id = @AnimalId;";
                    var idAnimal = listAnimals.SelectedValue;

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@AnimalId", idAnimal);
                    sqlCommand.ExecuteNonQuery();

                    MessageBox.Show($"Deleted the {(listAnimals.SelectedItem as DataRowView)?["Name"]} from the animal list");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception: " + ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                    ShowAnimals();
                }
            }


        }
        private void updateAnimal_click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=AlienPC;Initial Catalog=ZooDB;Persist Security Info=True;User ID=sa;Password=SQLDaMicrosoft;Pooling=False;Encrypt=True;Trust Server Certificate=True";
            using(SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    bool exists = listAnimals.Items.Cast<DataRowView>()
                    .Any(row => row["Name"].ToString().Equals(txtBoxZoo.Text, StringComparison.OrdinalIgnoreCase));

                    if (exists || string.IsNullOrEmpty(txtBoxZoo.Text))
                    {
                        MessageBox.Show("Cant't use duplicated names or edited to be empty");
                        return;
                    }

                    sqlConnection.Open();
                    //novo valor ao animal
                    string novoNomeAnimal = txtBoxZoo.Text.Trim();
                    novoNomeAnimal = char.ToUpper(novoNomeAnimal[0]) + novoNomeAnimal.Substring(1).ToLower();

                    //id do animal selecionado
                    var idAnimal = listAnimals.SelectedValue;

                    string query = "UPDATE Animal SET Name = @novoValor WHERE Id = @idCorrespondente;";
                    SqlCommand sqlCommand = new SqlCommand(query,sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@novoValor",novoNomeAnimal);
                    sqlCommand.Parameters.AddWithValue("@idCorrespondente", idAnimal);
                    sqlCommand.ExecuteNonQuery();


                }
                catch(Exception ex) 
                {
                    MessageBox.Show("Exception: " +ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                    ShowAnimals();
                    txtBoxZoo.Text = "";
                }
            }
        }
    }
}