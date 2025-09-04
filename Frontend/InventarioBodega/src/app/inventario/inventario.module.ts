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

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { MostrarInventarioComponent } from './mostrar-inventario/mostrar-inventario.component';

import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';

@NgModule({
  declarations: [
    RegistrarProductoComponent,
    MostrarVistaComponent,
    MostrarInventarioComponent
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
    NgxBarcode6Module,
    BrowserAnimationsModule,  
    ToastrModule.forRoot(),

    MatTableModule,
    MatPaginatorModule,
    MatSortModule
  ]
})
export class InventarioModule { }