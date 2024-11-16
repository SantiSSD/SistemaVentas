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
    public partial class mdProveedor : Form
    {

        public Proveedor _Proveedor { get; set; }



        public mdProveedor()
        {
            InitializeComponent();
        }

        private void mdProveedor_Load(object sender, EventArgs e)
        {
            HashSet<int> ProveedoresAgregados = new HashSet<int>();
            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible == true)
                {
                    cboBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 0;

            List<Proveedor> listaProveedor = new CN_Proveedor().Listar();

            foreach (Proveedor proveedor in listaProveedor)
            {
                if (!ProveedoresAgregados.Contains(proveedor.IdProveedor))
                {
                    // Si no está, agregar al DataGridView
                    dgvData.Rows.Add(new object[]
                    {

                       proveedor.IdProveedor,
                       proveedor.Documento,
                       proveedor.RazonSocial,

                    });

                    // Añadir al HashSet
                    ProveedoresAgregados.Add(proveedor.IdProveedor);
                }
            }
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            int iColum = e.ColumnIndex;

            if (iRow >= 0 && iColum > 0)
            {
                _Proveedor = new Proveedor()
                {
                    IdProveedor = Convert.ToInt32(dgvData.Rows[iRow].Cells["Id"].Value.ToString()),
                    Documento = (dgvData.Rows[iRow].Cells["Documento"].Value.ToString()),
                    RazonSocial = (dgvData.Rows[iRow].Cells["RazonSocial"].Value.ToString())

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
