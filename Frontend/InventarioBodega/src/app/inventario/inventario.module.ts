import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// Angular Material
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';

// Extras
import { NgxBarcode6Module } from 'ngx-barcode6';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

// Componentes inventario
import { RegistrarProductoComponent } from './registrar-producto/registrar-producto.component';
import { MostrarVistaComponent } from './mostrar-vista/mostrar-vista.component';
import { MostrarInventarioComponent } from './mostrar-inventario/mostrar-inventario.component';
import { PanelInventarioComponent } from './panel-inventario/panel-inventario.component';
import { InventarioRoutingModule } from './inventario-routing.module';
import { MostrarEntradasComponent } from './mostrar-entradas/mostrar-entradas.component';

@NgModule({
  declarations: [
    RegistrarProductoComponent,
    MostrarVistaComponent,
    MostrarInventarioComponent,
    PanelInventarioComponent,
    MostrarEntradasComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    InventarioRoutingModule,

    // Angular material
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatOptionModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,

    NgxBarcode6Module, 
    ToastrModule.forRoot(),

    
  ]
})
export class InventarioModule { }