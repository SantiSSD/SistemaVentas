using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Modales;
using CapaPresentacion.Utilidades;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmCompras : Form
    {
        private Usuario _Usuario;


        public frmCompras(Usuario oUsuario = null)
        {
            _Usuario = oUsuario;
            InitializeComponent();
        }

        private void frmCompras_Load(object sender, EventArgs e)
        {
            cbotipoDocumento.Items.Add(new OpcionCombo() { Valor = "Boleta", Texto = "Boleta" });
            cbotipoDocumento.Items.Add(new OpcionCombo() { Valor = "Factura", Texto = "Factura" });
            cbotipoDocumento.DisplayMember = "Texto";
            cbotipoDocumento.ValueMember = "Valor";
            cbotipoDocumento.SelectedIndex = 0;


            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txtIdproveedor.Text = "0";
            txtidProduto.Text = "0";

        }

        private void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            using (var modal = new mdProveedor())
            {
                var result = modal.ShowDialog();



                if (result == DialogResult.OK)
                {
                    txtIdproveedor.Text = modal._Proveedor.IdProveedor.ToString();
                    txtDocProveedor.Text = modal._Proveedor.Documento;
                    txtnombreProveedor.Text = modal._Proveedor.RazonSocial;
                }
                else
                {
                    txtDocProveedor.Select();
                }

            }
        }

        private void btnbuscarProducto_Click(object sender, EventArgs e)
        {
            using (var modal = new mdProducto())
            {
                var result = modal.ShowDialog();



                if (result == DialogResult.OK)
                {
                    txtidProduto.Text = modal._Producto.IdProducto.ToString();
                    txtcodproducto.Text = modal._Producto.Codigo;
                    txtproducto.Text = modal._Producto.Nombre;
                    txtprecioCompra.Select();
                }
                else
                {
                    txtcodproducto.Select();
                }
            }

        }

        private void txtcodproducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Producto oProducto = new CN_Producto().Listar().Where(p => p.Codigo == txtcodproducto.Text && p.Estado == true).FirstOrDefault();

                if (oProducto != null)
                {
                    txtcodproducto.BackColor = System.Drawing.Color.LimeGreen;
                    txtidProduto.Text = oProducto.IdProducto.ToString();
                    txtproducto.Text = oProducto.Nombre;
                    txtprecioCompra.Select();
                }
                else
                {
                    txtcodproducto.BackColor = System.Drawing.Color.DarkRed;
                    txtidProduto.Text = "0";
                    txtproducto.Text = "";
                }
            }
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            decimal preciocompra = 0;
            decimal precioventa = 0;
            bool producto_Existente = false;

            if (int.Parse(txtidProduto.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un producto ", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!decimal.TryParse(txtprecioCompra.Text, out preciocompra))
            {
                MessageBox.Show("Precio Compra - Formato moneda incorrecto ", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtprecioCompra.Select();
                return;
            }


            if (!decimal.TryParse(txtPrecioVenta.Text, out precioventa))
            {
                MessageBox.Show("Precio Venta - Formato moneda incorrecto ", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPrecioVenta.Select();
                return;
            }

            foreach (DataGridViewRow fila in dgvData.Rows)
            {
                if (fila.Cells["IdProducto"].Value.ToString() == txtidProduto.Text)
                {
                    producto_Existente = true;
                    break;
                }
            }

            if (!producto_Existente)
            {
                dgvData.Rows.Add(new object[]
                {
                  txtidProduto.Text,
                  txtproducto.Text,
                  preciocompra.ToString("0.00"),
                  precioventa.ToString("0.00"),
                  txtCantidad.Value.ToString(),
                  (txtCantidad.Value * preciocompra).ToString("0.00")

                });

                calcularTotal();
                limpiarProducto();
                txtcodproducto.Select();

            }
        }
        private void limpiarProducto()
        {
            txtidProduto.Text = "0";
            txtcodproducto.Text = "";
            txtcodproducto.BackColor = System.Drawing.Color.White;
            txtproducto.Text = "";
            txtprecioCompra.Text = "";
            txtPrecioVenta.Text = "";
            txtCantidad.Value = 1;

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

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            {
                if (e.RowIndex < 0)

                    return;
                if (e.ColumnIndex == 6)
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

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

        private void txtprecioCompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (txtprecioCompra.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void txtPrecioVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (txtPrecioVenta.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtIdproveedor.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un proveedor", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (dgvData.Rows.Count < 1)
            {
                MessageBox.Show("Debe ingresar productos en la compra", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable detalle_compra = new DataTable();

            detalle_compra.Columns.Add("IdProducto", typeof(int));
            detalle_compra.Columns.Add("PrecioCompra", typeof(decimal));
            detalle_compra.Columns.Add("PrecioVenta", typeof(decimal));
            detalle_compra.Columns.Add("Cantidad", typeof(int));
            detalle_compra.Columns.Add("SubTotal", typeof(decimal));


            foreach (DataGridViewRow row in dgvData.Rows)
            {
                detalle_compra.Rows.Add
                    (
                     new object[]
                     {
                        Convert.ToInt32(row.Cells["IdProducto"].Value.ToString()),
                        row.Cells["PrecioCompra"].Value.ToString(),
                        row.Cells["PrecioVenta"].Value.ToString(),
                        row.Cells["Cantidad"].Value.ToString(),
                        row.Cells["SubTotal"].Value.ToString(),
                    });

            }

            int idcorrelativo = new CN_Compra().ObtenerCorrelativo();
            string numerodocumento = string.Format("{0:00000}", idcorrelativo);

            Compra oCompra = new Compra()
            {
              oUsuario = new Usuario() { IdUsuario = _Usuario.IdUsuario },
              oProveedor = new Proveedor() {IdProveedor = Convert.ToInt32(txtIdproveedor.Text) },
              TipoDocumento = ((OpcionCombo)cbotipoDocumento.SelectedItem).Texto,
              NumeroDocumento = numerodocumento,
              MontoTotal = Convert.ToDecimal(txtTotalPagar.Text)
            };


            string mensaje = string.Empty;
            bool respuesta = new CN_Compra().Registrar(oCompra, detalle_compra, out mensaje);

            if (respuesta)
            {
                var result = MessageBox.Show("Número de compra generada:\n" + numerodocumento + "\n\n¿Desea copiar al" +
                    " portapapeles?", "Mensaje", MessageBoxButtons.YesNo,MessageBoxIcon.Information);
                
                if (result == DialogResult.Yes)
                    Clipboard.SetText(numerodocumento);


                txtIdproveedor.Text = "0";
                txtDocProveedor.Text = "";
                txtnombreProveedor.Text = "";
                dgvData.Rows.Clear();
                calcularTotal();

            }
            else
            {
                MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }




        }
    }
}
