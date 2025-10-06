import { Component } from '@angular/core';
import { InventarioService, Inventario } from '../../services/inventario.service';
import { ToastrService } from 'ngx-toastr';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-registrar-producto',
  templateUrl: './registrar-producto.component.html',
  styleUrls: ['./registrar-producto.component.css']
})
export class RegistrarProductoComponent {

  loading = false;

  producto: Inventario = {
    id: 0,
    referencia: '',
    descripcion: '',
    tipoBolsa: '',
    tipoMaterial: '',
    densidad: '',
    color: '',
    segundoColor: '',
    impresoNo: '',
    ancho: 0,
    alto: 0,
    calibre: 0,
    peso: 0
  };

  constructor(
    private inventarioService: InventarioService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<RegistrarProductoComponent>
  ) {}

  RegistrarProducto() {
    if (!this.producto.referencia) {
      this.toastr.error('La referencia es obligatoria', 'Error');
      return;
    }

    this.loading = true;

    const payload = {
      Referencia: this.producto.referencia,
      Descripcion: this.producto.descripcion,
      TipoBolsa: this.producto.tipoBolsa,
      TipoMaterial: this.producto.tipoMaterial,
      ImpresoNo: this.producto.impresoNo,
      Ancho: this.producto.ancho,
      Alto: this.producto.alto,
      Calibre: this.producto.calibre,
      Color: this.producto.color,
      SegundoColor: this.producto.segundoColor,
      Densidad: this.producto.densidad
    };

    this.inventarioService.addInventario(payload).subscribe({
      next: () => {
        this.toastr.success('Producto registrado con éxito', 'Éxito');
        this.dialogRef.close(true); 
      },
      error: (err) => {
        console.error('Error al registrar producto:', err);
        this.toastr.error('Error al registrar el producto', 'Error');
      },
      complete: () => {
        this.loading = false;
      }
    });
  }

  cancelar() {
    this.dialogRef.close(false);
  }
}
