 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;
using CapaEntidad;
using FontAwesome.Sharp;
using DocumentFormat.OpenXml.Presentation;

namespace CapaPresentacion
{
    public partial class Inicio : Form
    {
        private static Usuario usuarioActual;
        private static IconMenuItem MenuActivo = null;
        private static Form FormularioActivo = null;

        public Inicio(Usuario objUsuario = null)
        {
            if (objUsuario == null)
                usuarioActual = new Usuario() { NombreCompleto = "ADMIN PREDEFINIDO", IdUsuario = 1 };
            else
                usuarioActual = objUsuario;

            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.Sizable; // Permitir redimensionamiento
            this.MinimumSize = new Size(800, 600); // Tamaño mínimo del formulario
            this.Resize += Inicio_Resize; // Manejar el evento Resize
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            List<Permiso> ListaPermisos = new CN_Permiso().Listar(usuarioActual.IdUsuario);

            foreach (IconMenuItem iconMenu in Menu.Items)
            {
                bool encontrado = ListaPermisos.Any(m => m.NombreMenu == iconMenu.Name);
                if (!encontrado)
                {
                    iconMenu.Visible = false;
                }
            }

            lblUsuario.Text = usuarioActual.NombreCompleto;
        }

        private void AbrirFormulario(IconMenuItem menu, Form formulario)
        {
            if (MenuActivo != null)
            {
                MenuActivo.BackColor = Color.FromArgb(31, 30, 68);
            }
            menu.BackColor = Color.Purple;
            MenuActivo = menu;

            if (FormularioActivo != null)
            {
                FormularioActivo.Close();
            }

            FormularioActivo = formulario;
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill; // Ajuste automático al tamaño del contenedor
            formulario.BackColor = Color.FromArgb(31, 30, 68);

            Contenedor.Controls.Add(formulario);
            Contenedor.Tag = formulario;
            formulario.Show();
        }

        private void Inicio_Resize(object sender, EventArgs e)
        {
            // Ajustar el formulario activo al nuevo tamaño del contenedor
            if (FormularioActivo != null)
            {
                FormularioActivo.Dock = DockStyle.Fill;
            }
        }


        private void MenuUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmUsuarios());
        }

        private void SubMenuCategoria_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MenuMantenedor, new frmCategoria());
        }

        private void SubMenuProducto_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MenuMantenedor, new frmProducto());
        }

        private void SubMenuRegistrarVenta_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MenuVentas, new frmVentas(usuarioActual));
        }

        private void SubMenuVerDetalleVenta_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MenuVentas, new frmDetalleVenta());
        }

        private void SubMenuRegistrarCompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MenuCompras, new frmCompras(usuarioActual));
        }

        private void SubMenuVerDetalleCompras_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MenuCompras, new frmDetalleCompra());
        }

        private void MenuClientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmClientes());
        }

        private void MenuProveedores_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmProveedores());
        }

        private void MenuReportes_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmReportes());
        }

        private void SubMenuNegocio_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MenuMantenedor, new frmNegocio());
        }

   
    }
}

