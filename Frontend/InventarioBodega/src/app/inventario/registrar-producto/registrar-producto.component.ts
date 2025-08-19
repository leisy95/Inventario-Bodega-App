import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

interface TipoProducto {
  idTipo: number;
  referencia: string;
  color?: string;
  medida?: string;
  unidadPeso?: string;
  peso?: number;
  ancho?: number;
  largo?: number;
  calibre?: number;
  material?: string;
  mezcla?: string;
  toquel?: string;
}

@Component({
  selector: 'app-registrar-producto',
  templateUrl: './registrar-producto.component.html',
  styleUrls: ['./registrar-producto.component.css']
})
export class RegistrarProductoComponent implements OnInit {

  // --- Campos para registro ---
  referenciaTipo: string = '';
  color: string = '';
  medida: string = '';
  unidadPeso: string = 'kg';
  peso: number | null = null;
  ancho: string = '';
  largo: string = '';
  calibre: string = '';
  material: string = '';
  mezcla: string = '';
  toquel: string = '';

  // --- Estados para registro ---
  generatedItemId: number | null = null;
  barcodeImageUrl: string = '';
  loading: boolean = false;
  message: string = '';

  // --- Para mostrar datos ---
  tiposProducto: TipoProducto[] = [];
  loadingTipos = false;
  errorTipos: string | null = null;

  private readonly API_BASE_URL = 'http://localhost:5244/api/Inventario';

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    // Puedes cargar datos automáticamente aquí si quieres
  }

  generateBarcodeImage(id: number): string {
    return `https://placehold.co/300x100/000/fff?text=ID:${id}`;
  }

  async handleRegisterItem(): Promise<void> {
    if (!this.referenciaTipo || !this.unidadPeso) {
      this.message = 'Por favor, complete la Referencia (Tipo) y la Unidad de Peso.';
      return;
    }

    this.loading = true;
    this.message = '';
    this.generatedItemId = null;
    this.barcodeImageUrl = '';

    const requestBody = {
      referenciaTipo: this.referenciaTipo,
      color: this.color,
      medida: this.medida,
      unidadPeso: this.unidadPeso,
      peso: this.peso,
      ancho: this.ancho,
      largo: this.largo,
      calibre: this.calibre,
      material: this.material,
      mezcla: this.mezcla,
      toquel: this.toquel
    };

    try {
      const data: any = await this.http.post(`${this.API_BASE_URL}/registrar-item`, requestBody).toPromise();

      this.generatedItemId = data.idItemGenerado;
      this.barcodeImageUrl = this.generateBarcodeImage(data.idItemGenerado);
      this.message = `¡Éxito! Item ID ${data.idItemGenerado} generado para la referencia ${data.referenciaTipo}.`;

      this.referenciaTipo = '';
      this.color = '';
      this.medida = '';
      this.unidadPeso = 'kg';
      this.ancho = '';
      this.largo = '';
      this.calibre = '';
      this.material = '';
      this.mezcla = '';
      this.toquel = '';

    } catch (error: any) {
      console.error('Error de red o al procesar la solicitud:', error);
      if (error.error && error.error.message) {
        this.message = `Error: ${error.error.message}`;
      } else {
        this.message = 'Ocurrió un error de conexión. Asegúrese de que el backend esté funcionando y el puerto sea correcto.';
      }
    } finally {
      this.loading = false;
    }
  }

  handlePrintTicket(): void {
    if (!this.generatedItemId) {
      this.message = 'Primero debe generar un item para poder imprimir el tiquete.';
      return;
    }
    window.print();
    this.message = 'Se ha enviado la solicitud de impresión al navegador. Ajuste la configuración de la página para el tamaño del tiquete.';
  }

  // --- NUEVO: función para mostrar la lista de tipos de producto ---
   irMostrarVista() {
    this.router.navigate(['/mostrar-vista']);
  }
}