# 🦁 ZooManager

O **ZooManager** é um sistema de gestão para zoológicos desenvolvido com foco educacional utilizando **WPF (.NET Framework)** como interface gráfica e **Microsoft SQL Server** como sistema de persistência de dados. O objetivo do projeto é simular o controle administrativo de um zoológico, permitindo gerenciar animais, espécies, habitats e seus respectivos cuidadores.

---

## 🧰 Tecnologias Utilizadas

- 💻 **C# com WPF** (Windows Presentation Foundation)
- 🗃️ **Microsoft SQL Server**
- 🔌 **ADO.NET** (via `SqlConnection`, `SqlCommand`, `SqlDataReader`)
- 📦 **XAML** para a construção da interface
- ⚙️ **App.config** com `connectionString` para configuração da base de dados

---

## 🎯 Funcionalidades

- ✅ Cadastro e edição de animais e seus habitats
- ✅ Visualização dos dados em tempo real com DataGrids
- ✅ Exclusão segura de registros
- ✅ Interação com banco de dados via comandos SQL
- ✅ Interface WPF moderna e responsiva


---

## 🔌 Conexão com o Banco de Dados

A conexão com o banco é feita manualmente com ADO.NET, sem uso de ORMs como Entity Framework. A string de conexão fica configurada no `App.config`.

### Exemplo:

```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Data Source=localhost;Initial Catalog=ZooManagerDB;Integrated Security=True" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
