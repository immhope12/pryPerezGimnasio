using System;
using System.Windows.Forms;

namespace pryPerezGimnasio
{
    // STRUCT
    public struct Socio
    {

        public string Nombre;
        public int Edad;
        public bool EsEstudiante;
        public string Plan;
        public string Turno;
        public int Meses;
        public bool Casillero;
        public string FormaPago;
        public double CostoFinal;
    }

    public partial class frmInscripcion : Form
    {
        // ARRAY DE 1 DIMENSIÓN Y EL ÍNDICE
        Socio[] arrayClientes = new Socio[3];
        int indiceClientes = 0;

        public frmInscripcion()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void groupBox1_Enter_1(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void checkBox2_CheckedChanged(object sender, EventArgs e) { }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }
        private void lblTurno_Click(object sender, EventArgs e) { }
        private void lblEstudiante_Click(object sender, EventArgs e) { }

        // --- LÓGICA DE LA APP ---

        private void frmInscripcion_Load(object sender, EventArgs e)
        {
            RestablecerFormulario();
        }

        private void RestablecerFormulario()
        {
            txtNombre.Clear();
            txtEdad.Clear();
            txtMeses.Text = "1";
            cboxEstudiante.Checked = false;
            cboxCasillero.Checked = false;

            cboxPlan.SelectedIndex = 0;
            cboxTruno.SelectedIndex = 0;

            rbtEfectivo.Checked = true;
            cboxCuotas.Enabled = false;
            cboxCuotas.SelectedIndex = -1;


        }

        private void ValidarCamposCompletos(object sender, EventArgs e)
        {
            btnCalcular.Enabled = !string.IsNullOrWhiteSpace(txtNombre.Text) &&
                                  !string.IsNullOrWhiteSpace(txtEdad.Text) &&
                                  !string.IsNullOrWhiteSpace(txtMeses.Text);
        }

        private void txtNombre_TextChanged(object sender, EventArgs e) => ValidarCamposCompletos(sender, e);
        private void txtEdad_TextChanged(object sender, EventArgs e) => ValidarCamposCompletos(sender, e);
        private void txtMeses_TextChanged(object sender, EventArgs e) => ValidarCamposCompletos(sender, e);

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void SoloNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtTarjeta.Checked)
            {
                cboxCuotas.Enabled = true;
                if (cboxCuotas.Items.Count > 0) cboxCuotas.SelectedIndex = 0;
            }
            else
            {
                cboxCuotas.Enabled = false;
                cboxCuotas.SelectedIndex = -1;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            RestablecerFormulario();
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            Socio socio = new Socio();

            int edad = int.Parse(txtEdad.Text);
            if (edad < 14)
            {
                MessageBox.Show("La edad mínima es de 14 años. No puede inscribirse.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int meses = int.Parse(txtMeses.Text);
            if (meses < 1 || meses > 12)
            {
                MessageBox.Show("La cantidad de meses debe estar entre 1 y 12.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Precio base por plan
            double precioMensual = 0;
            switch (cboxPlan.Text)
            {
                case "Musculacion": precioMensual = 15000; break;
                case "Funcional": precioMensual = 18000; break;
                case "Natacion": precioMensual = 22000; break;
            }

            // Suma del casillero
            if (cboxCasillero.Checked)
            {
                precioMensual += 3000;
            }

            double subtotal = precioMensual * meses;

            // Descuentos
            double porcentajeDescuento = 0;
            if (edad >= 65) porcentajeDescuento = 0.30;
            else if (edad < 18) porcentajeDescuento = 0.25;
            else if (cboxEstudiante.Checked) porcentajeDescuento = 0.15;

            double subtotalConDescuento = subtotal - (subtotal * porcentajeDescuento);

            // Modificador de Pago
            double modificadorPago = 0;
            if (rbtEfectivo.Checked)
            {
                modificadorPago = -0.10;
            }
            else if (rbtTarjeta.Checked)
            {
                if (cboxCuotas.Text == "3") modificadorPago = 0.10;
                else if (cboxCuotas.Text == "6") modificadorPago = 0.20;
            }

            double costoFinal = subtotalConDescuento + (subtotalConDescuento * modificadorPago);

            socio.Nombre = txtNombre.Text;
            socio.Edad = edad;
            socio.Plan = cboxPlan.Text;
            socio.Turno = cboxTruno.Text;
            socio.Meses = meses;
            socio.CostoFinal = costoFinal;

            // Mostrar el mensaje
            string detalle = $"Socio: {txtNombre.Text}\n" +
                             $"Plan: {cboxPlan.Text} ({cboxTruno.Text})\n" +
                             $"Meses: {meses}\n" +
                             $"Costo Final: ${costoFinal:N2}";

            MessageBox.Show(detalle, "Detalle de Inscripción", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (indiceClientes < arrayClientes.Length)
            {
                arrayClientes[indiceClientes] = socio;
                indiceClientes++;
            }
            else
            {
                MessageBox.Show("No hay más lugar para guardar clientes.", "Array lleno",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            RestablecerFormulario();
        }

        private void tbpDatos_Click(object sender, EventArgs e)
        {

        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            lstClientes.Items.Clear();

            int indiceRecorrido = 0;
            while (indiceRecorrido < indiceClientes)
            {
                lstClientes.Items.Add(
                    "Nombre: " + arrayClientes[indiceRecorrido].Nombre +
                    " | Plan: " + arrayClientes[indiceRecorrido].Plan +
                    " | Meses: " + arrayClientes[indiceRecorrido].Meses +
                    " | Costo final: $" + arrayClientes[indiceRecorrido].CostoFinal.ToString("N2"));
                indiceRecorrido++;
            }
        }

        private void txtNombre_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}