import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Inventario, InventarioService } from '../../services/inventario.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-editar-inventario',
  templateUrl: './editar-inventario.component.html',
  styleUrls: ['./editar-inventario.component.css']
})
export class EditarInventarioComponent {
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private inventarioService: InventarioService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<EditarInventarioComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Inventario
  ) {
    this.form = this.fb.group({
      referencia: [this.data.referencia, Validators.required],
      descripcion: [this.data.descripcion || ''],
      tipoBolsa: [this.data.tipoBolsa],
      tipoMaterial: [this.data.tipoMaterial],
      densidad: [this.data.densidad],
      color: [this.data.color],
      segundoColor: [this.data.segundoColor],
      impresoNo: [this.data.impresoNo],
      ancho: [this.data.ancho, Validators.required],
      alto: [this.data.alto, Validators.required],
      calibre: [this.data.calibre],
      peso: [{ value: this.data.peso, disabled: true }],
      cantidad:  [{ value: this.data.cantidad, disabled: true }]
    });
  }

  guardar() {
    if (this.form.invalid) {
      this.toastr.error('Completa todos los campos requeridos');
      return;
    }

    const inventarioActualizado = {
      id: this.data.id,                 
      referencia: this.form.value.referencia,
      descripcion: this.form.value.descripcion || '',
      tipoBolsa: this.form.value.tipoBolsa || '',
      tipoMaterial: this.form.value.tipoMaterial || '',
      densidad: this.form.value.densidad || '',
      color: this.form.value.color || '',
      segundoColor: this.form.value.segundoColor || '',
      impresoNo: this.form.value.impresoNo || '',
      ancho: Number(this.form.value.ancho) || 0,
      alto: Number(this.form.value.alto) || 0,
      calibre: Number(this.form.value.calibre) || 0,
      peso: this.data.peso,         //  no se edita
      cantidad: this.data.cantidad, //  no se edita
      referenciaNormalizada: this.data.referenciaNormalizada || '',
      inventarioItems: []
    };

    // Llamar al servicio para actualizar
    this.inventarioService.updateInventario(this.data.id, inventarioActualizado)
      .subscribe({
        next: () => {
          this.toastr.success('Inventario actualizado correctamente', 'Éxito');
          this.dialogRef.close(true);
        },
        error: (err) => {
          this.toastr.error('Error al actualizar: ' + (err.error?.mensaje || err.message), 'Error');
        }
      });
  }

  cancelar() {
    this.dialogRef.close(false);
  }
}