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

namespace CapaPresentacion
{
    public partial class frmProveedores : Form
    {
        public frmProveedores()
        {
            InitializeComponent();
        }

        private void frmProveedores_Load(object sender, EventArgs e)
        {
            dgvData.Rows.Clear();
            // Inicializar una lista de IDs para verificar duplicados
            HashSet<int> ProveedoresAgregados = new HashSet<int>(); // Aquí se crea el HashSet

            cboEstado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;



            cboBusqueda.Items.Clear();
            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible == true && columna.Name != "btnSeleccionar")
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
                        "",
                       proveedor.IdProveedor,
                       proveedor.Documento,
                       proveedor.RazonSocial,
                       proveedor.Correo,
                       proveedor.Telefono,
                       proveedor.Estado == true ? 1 : 0,
                       proveedor.Estado == true ? "Activo" : "Inactivo",
                    });

                    // Añadir al HashSet
                    ProveedoresAgregados.Add(proveedor.IdProveedor);
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            // Creación del objeto cliente
            Proveedor obj = new Proveedor()
            {    // Validación previa para convertir txtId.Text a entero
                IdProveedor = string.IsNullOrWhiteSpace(txtId.Text) ? 0 : Convert.ToInt32(txtId.Text.Trim()),
                Documento = txtDocumento.Text.Trim(),
                RazonSocial = txtRazonSocial.Text.Trim(),
                Correo = txtCorreo.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).Valor) == 1
            };

            if (obj.IdProveedor == 0)
            {
                // Registrar nuevo cliente
                int idgenerado = new CN_Proveedor().Registrar(obj, out mensaje);

                if (idgenerado != 0)
                {
                    dgvData.Rows.Add(new object[]
                    {
                "",
                idgenerado,
                obj.Documento,
                obj.RazonSocial,
                obj.Correo,
                obj.Telefono,
                ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString(),
                ((OpcionCombo)cboEstado.SelectedItem).Texto.ToString()
                    });

                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje);
                }
            }
            else
            {
                // Editar cliente existente
                bool resultado = new CN_Proveedor().Editar(obj, out mensaje);

                if (resultado)
                {
                    DataGridViewRow row = dgvData.Rows[Convert.ToInt32(txtIndice.Text)];
                    row.Cells["Id"].Value = txtId.Text;
                    row.Cells["Documento"].Value = obj.Documento;
                    row.Cells["RazonSocial"].Value = obj.RazonSocial;
                    row.Cells["Correo"].Value = obj.Correo;
                    row.Cells["Telefono"].Value = obj.Telefono;
                    row.Cells["EstadoValor"].Value = ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombo)cboEstado.SelectedItem).Texto.ToString();

                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje);
                }
            }
        }
        private void Limpiar()
        {
            txtIndice.Text = "-1";
            txtId.Text = ""; // Vaciar el campo
            txtDocumento.Text = ""; // Vaciar el campo
            txtRazonSocial.Text = ""; // Vaciar el campo
            txtCorreo.Text = ""; // Vaciar el campo
            txtTelefono.Text = ""; // Vaciar el campo
            cboEstado.SelectedIndex = 0; // Seleccionar el primer ítem de la lista
            txtDocumento.Select();

        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)

                return;
            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.check20.Width;
                var h = Properties.Resources.check20.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.check20, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtId.Text = dgvData.Rows[indice].Cells["Id"].Value.ToString();
                    txtDocumento.Text = dgvData.Rows[indice].Cells["Documento"].Value.ToString();
                    txtRazonSocial.Text = dgvData.Rows[indice].Cells["RazonSocial"].Value.ToString();
                    txtCorreo.Text = dgvData.Rows[indice].Cells["Correo"].Value.ToString();
                    txtTelefono.Text = dgvData.Rows[indice].Cells["Telefono"].Value.ToString();



                    foreach (OpcionCombo oc in cboEstado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["EstadoValor"].Value))
                        {
                            int indice_combo = cboEstado.Items.IndexOf(oc);
                            cboEstado.SelectedIndex = indice_combo;
                            break;
                        }
                    }
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtId.Text) != 0)
            {
                if (MessageBox.Show("¿Deseas eliminar este proveedor?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    Proveedor obj = new Proveedor()
                    {
                        IdProveedor = Convert.ToInt32(txtId.Text)
                    };
                    bool respuesta = new CN_Proveedor().Eliminar(obj, out mensaje);

                    if (respuesta)
                    {
                        dgvData.Rows.RemoveAt(Convert.ToInt32(txtIndice.Text));
                        Limpiar();

                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnLimpiarBuscador_Click(object sender, EventArgs e)
        {
            {
                txtBusqueda.Text = "";
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    row.Visible = true;
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
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
}