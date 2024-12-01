using CapaEntidad;
using CapaNegocio;
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

namespace CapaPresentacion.Modales
{
    public partial class mdCliente : Form
    {

        public Cliente _Cliente { get; set; }
        public mdCliente()
        {
            InitializeComponent();
        }

        private void mdCliente_Load(object sender, EventArgs e)
        {
            HashSet<int> ClientesAgregados = new HashSet<int>();
            foreach (DataGridViewColumn columna in dgvData.Columns)
            {


                cboBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });

            }
            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 0;

            List<Cliente> lista = new CN_Cliente().Listar();

            foreach (Cliente cliente in lista)
            {
                if (cliente.Estado)
                {
                    dgvData.Rows.Add(new object[] { cliente.Documento, cliente.NombreCompleto });
                }


            }

        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            int iColum = e.ColumnIndex;

            if (iRow >= 0 && iColum > 0)
            {
                _Cliente = new Cliente()
                {
                    Documento = (dgvData.Rows[iRow].Cells["Documento"].Value.ToString()),
                    NombreCompleto = dgvData.Rows[iRow].Cells["NombreCompleto"].Value.ToString(),

                };
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            {
                string columnaFiltro = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();

                if (dgvData.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in dgvData.Rows)
                    {
                        if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))

                            row.Visible = true;

                        else

                            row.Visible = false;

                    }
                }
            }
        }

        private void btnLimpiarBuscador_Click(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                row.Visible = true;
            }
        }
    }
}


