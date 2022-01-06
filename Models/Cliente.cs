using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Models
{
    public class Cliente
    {
        public Cliente() { }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }

        public static List<Cliente> Todos()
        {
            var lista = new List<Cliente>();
            SqlConnection conn = new SqlConnection(Conexao.Dados);
            conn.Open();

            SqlCommand cmd = new SqlCommand("select * from clientes", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Cliente
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nome = reader["Nome"].ToString(),
                    Endereco = reader["Endereco"].ToString(),
                    Telefone = reader["Telefone"].ToString()
                });
            }
            conn.Close();
            conn.Dispose(); // garbage collection
            cmd.Dispose(); //garbage
            return lista;
        }

        public Cliente Salvar()
        {
            using (SqlConnection conn = new SqlConnection(Conexao.Dados))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("CriarCliente @nome, @endereco, @telefone", conn);

                //SqlCommand cmd = new SqlCommand("insert into clientes(Nome, Endereco, Telefone) values(@nome, @endereco, @telefone)", conn);
                cmd.Parameters.Add("@nome", SqlDbType.VarChar);
                cmd.Parameters["@nome"].Value = this.Nome;
                cmd.Parameters.Add("@endereco", SqlDbType.VarChar);
                cmd.Parameters["@endereco"].Value = this.Endereco;
                cmd.Parameters.Add("@telefone", SqlDbType.VarChar);
                cmd.Parameters["@telefone"].Value = this.Telefone;
                this.Id = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            return this;
        }
    }
}
