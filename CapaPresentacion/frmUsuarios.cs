using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;
namespace CapaPresentacion
{
    public partial class frmUsuarios : Form
    {
        public frmUsuarios()
        {
            InitializeComponent();
        }

        private void frmUsuarios_Load(object sender, EventArgs e)
        {

            cboEstado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;


            List<Rol> listaRol = new CN_Rol().Listar();

            foreach (Rol item in listaRol)
            {
                cboRol.Items.Add(new OpcionCombo() { Valor = item.IdRol, Texto = item.Descripción });
                cboRol.DisplayMember = "Texto";
                cboRol.ValueMember = "Valor";
                cboRol.SelectedIndex = 0;

                cboBusqueda.Items.Clear();
                foreach  ( DataGridViewColumn columna  in dgvData.Columns)
                {
                    if (columna.Visible == true && columna.Name != "btnSeleccionar" )
                    {
                        cboBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText});
                    }
                }
                cboBusqueda.DisplayMember = "Texto";
                cboBusqueda.ValueMember = "Valor";
                cboBusqueda.SelectedIndex = 0;

            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            dgvData.Rows.Add(new object[] {"" , txtId.Text, txtDocumento.Text, txtNombreCompleto.Text, txtCorreo.Text, txtClave.Text,
             ( (OpcionCombo) cboRol.SelectedItem).Valor.ToString(),
             ( (OpcionCombo) cboRol.SelectedItem).Texto.ToString(),
             ( (OpcionCombo) cboEstado.SelectedItem).Valor.ToString(),
             ( (OpcionCombo) cboEstado.SelectedItem).Texto.ToString(),
            });

            Limpiar();
        }
        private void Limpiar()
        {
            txtId.Text = ""; // Vaciar el campo
            txtDocumento.Text = ""; // Vaciar el campo
            txtNombreCompleto.Text = ""; // Vaciar el campo
            txtCorreo.Text = ""; // Vaciar el campo
            txtClave.Text = ""; // Vaciar el campo
            txtConfirmarClave.Text = "";
            cboRol.SelectedIndex = 0; // Seleccionar el primer ítem de la lista
            cboEstado.SelectedIndex = 0; // Seleccionar el primer ítem de la lista



        }
    }
}

