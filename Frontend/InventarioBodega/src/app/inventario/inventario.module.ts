import { CommonModule } from '@angular/common';
import { MostrarVistaComponent } from './mostrar-vista/mostrar-vista.component';
import { NgModule } from '@angular/core';

import { RegistrarProductoComponent } from './registrar-producto/registrar-producto.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { NgxBarcode6Module } from 'ngx-barcode6';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core'

@NgModule({
  declarations: [
    RegistrarProductoComponent,
    MostrarVistaComponent,
  ],
  imports: [
    CommonModule,
     FormsModule,
     // Angular material
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatOptionModule,
    NgxBarcode6Module
  ]
})
export class InventarioModule { }