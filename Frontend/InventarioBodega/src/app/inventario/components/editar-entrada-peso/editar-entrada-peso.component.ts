
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-editar-entrada-peso',
  templateUrl: './editar-entrada-peso.component.html',
  styleUrls: ['./editar-entrada-peso.component.css']
})
export class EditarEntradaPesoComponent {
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<EditarEntradaPesoComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { pesoActual: number }
  ) {
    this.form = this.fb.group({
      pesoActual: [data.pesoActual, [Validators.required, Validators.min(0.01)]]
    });
  }

  guardar(): void {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value.pesoActual);
    }
  }

  cancelar(): void {
    this.dialogRef.close();
  }
}