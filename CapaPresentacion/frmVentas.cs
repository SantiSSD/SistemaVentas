using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Modales;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmVentas : Form
    {
        private Usuario _Usuario;
        public frmVentas(Usuario oUsuario = null)
        {
            _Usuario = oUsuario;
            InitializeComponent();
        }

        private void frmVentas_Load(object sender, EventArgs e)
        {
            cboTipoDoc.Items.Add(new OpcionCombo() { Valor = "Boleta", Texto = "Boleta" });
            cboTipoDoc.Items.Add(new OpcionCombo() { Valor = "Factura", Texto = "Factura" });
            cboTipoDoc.DisplayMember = "Texto";
            cboTipoDoc.ValueMember = "Valor";
            cboTipoDoc.SelectedIndex = 0;

            txtIdProducto.Text = "0";

            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txtPagaCon.Text = "";
            txtCambio.Text = "";
            txtTotalPagar.Text = "0";
        }

        private void btnBuscarCLiente_Click(object sender, EventArgs e)
        {
            using (var modal = new mdCliente())
            {
                var result = modal.ShowDialog();



                if (result == DialogResult.OK)
                {
                    txtdocCliente.Text = modal._Cliente.Documento;
                    txtNombreCliente.Text = modal._Cliente.NombreCompleto;
                    txtCodProducto.Select();
                }
                else
                {
                    txtdocCliente.Select();
                }

            }
        }

        private void btnBuscarProducto_Click_1(object sender, EventArgs e)
        {
            using (var modal = new mdProducto())
            {
                var result = modal.ShowDialog();



                if (result == DialogResult.OK)
                {
                    txtIdProducto.Text = modal._Producto.IdProducto.ToString();
                    txtCodProducto.Text = modal._Producto.Codigo;
                    txtNombreProducto.Text = modal._Producto.Nombre;
                    txtPrecioProducto.Text = modal._Producto.PrecioVenta.ToString("0.00");
                    txtStock.Text = modal._Producto.Stock.ToString();
                    txtCantidad.Select();
                }
                else
                {
                    txtCodProducto.Select();
                }
            }

        }

        private void txtCodProducto_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Producto oProducto = new CN_Producto().Listar().Where(p => p.Codigo == txtCodProducto.Text && p.Estado == true).FirstOrDefault();

                if (oProducto != null)
                {
                    txtCodProducto.BackColor = System.Drawing.Color.LimeGreen;
                    txtIdProducto.Text = oProducto.IdProducto.ToString();
                    txtNombreProducto.Text = oProducto.Nombre;
                    txtPrecioProducto.Text = oProducto.PrecioVenta.ToString("0.00");
                    txtStock.Text = oProducto.Stock.ToString();
                    txtCantidad.Select();
                }
                else
                {
                    txtCodProducto.BackColor = System.Drawing.Color.DarkRed;
                    txtIdProducto.Text = "0";
                    txtNombreProducto.Text = "";
                    txtPrecioProducto.Text = "";
                    txtStock.Text = "";
                    txtCantidad.Value = 1;
                }
            }
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            decimal precio = 0;
            bool producto_Existente = false;

            if (int.Parse(txtIdProducto.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un producto ", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!decimal.TryParse(txtPrecioProducto.Text, out precio))
            {
                MessageBox.Show("Precio - Formato moneda incorrecto ", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPrecioProducto.Select();
                return;
            }


            if (Convert.ToInt32(txtStock.Text) < Convert.ToInt32(txtCantidad.Value.ToString()))
            {
                MessageBox.Show("La cantidad no puede ser mayor al stock", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (DataGridViewRow fila in dgvData.Rows)
            {
                if (fila.Cells["IdProducto"].Value.ToString() == txtIdProducto.Text)
                {
                    producto_Existente = true;
                    break;
                }
            }

            if (!producto_Existente)
            {
                dgvData.Rows.Add(new object[]
                {
                  txtIdProducto.Text,
                  txtNombreProducto.Text,
                  precio.ToString("0.00"),
                  txtCantidad.Value.ToString(),
                  (txtCantidad.Value * precio).ToString("0.00")

                });
                calcularTotal();
                limpiarProducto();
                txtCodProducto.Select();
            }
        }
        private void calcularTotal()
        {
            decimal total = 0;
            if (dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                    total += Convert.ToDecimal(row.Cells["SubTotal"].Value.ToString());
            }

            txtTotalPagar.Text = total.ToString("0.00");
        }
        private void limpiarProducto()
        {
            txtIdProducto.Text = "0";
            txtCodProducto.Text = "";
            txtNombreProducto.Text = "";
            txtPrecioProducto.Text = "";
            txtStock.Text = "";
            txtCantidad.Value = 1;

        }

        private void dgvData_CellPainting_1(object sender, DataGridViewCellPaintingEventArgs e)
        {
            {
                if (e.RowIndex < 0)

                    return;
                if (e.ColumnIndex == 5)
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                    var w = Properties.Resources.delete25.Width;
                    var h = Properties.Resources.delete25.Height;
                    var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                    var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                    e.Graphics.DrawImage(Properties.Resources.delete25, new Rectangle(x, y, w, h));
                    e.Handled = true;
                }

            }
        }

        private void dgvData_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            {
                if (dgvData.Columns[e.ColumnIndex].Name == "btnEliminar")
                {
                    int indice = e.RowIndex;

                    if (indice >= 0)
                    {
                        dgvData.Rows.RemoveAt(indice);
                        calcularTotal();

                    }
                }
            }


        }

        private void txtPrecioProducto_KeyPress(object sender, KeyPressEventArgs e)
        {
            {
                string separadorDecimal = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                if (char.IsDigit(e.KeyChar) || e.KeyChar.ToString() == separadorDecimal || char.IsControl(e.KeyChar))
                {
                    e.Handled = false; // Permitir entrada
                }
                else
                {
                    e.Handled = true; // Bloquear entrada
                }
            }
        }


        private void txtPagaCon_KeyPress(object sender, KeyPressEventArgs e)
        {
            {
                // Obtén el separador decimal de la cultura actual
                string separadorDecimal = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                // Permitir dígitos, separador decimal y teclas de control como retroceso
                if (char.IsDigit(e.KeyChar) || e.KeyChar.ToString() == separadorDecimal || char.IsControl(e.KeyChar))
                {
                    e.Handled = false; // Permitir entrada
                }
                else
                {
                    e.Handled = true; // Bloquear entrada
                }

            }
        }
    
        private void calcularCambio()
        {
            if (string.IsNullOrWhiteSpace(txtTotalPagar.Text) || !decimal.TryParse(txtTotalPagar.Text, out decimal total))
            {
                MessageBox.Show("No existen productos a la venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!decimal.TryParse(txtPagaCon.Text.Trim(), out decimal pagaCon))
            {
                MessageBox.Show("Monto ingresado inválido", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPagaCon.Text = "0.00";
                txtCambio.Text = "0.00";
                return;
            }

            if (pagaCon < total)
            {
                txtCambio.Text = "0.00";
            }
            else
            {
                decimal cambio = pagaCon - total;
                txtCambio.Text = cambio.ToString("0.00");
            }

        }

        private void txtPagaCon_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                calcularCambio();
            }
        }

        private void txtPrecioProducto_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtPrecioProducto.Text, out decimal valor))
            {
                txtPrecioProducto.Text = valor.ToString("0.00");
            }
            else
            {
                txtPrecioProducto.Text = "0.00"; // Valor por defecto si no es válido
            }
        }

        private void txtPagaCon_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtPagaCon.Text, out decimal valor))
            {
                txtPagaCon.Text = valor.ToString("0.00"); // Formato con dos decimales
            }
            else
            {
                txtPagaCon.Text = "0.00"; // Valor por defecto si el ingreso no es válido
            }
        }
    }
}







