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
import { RegistrarProductoComponent } from './components/registrar-producto/registrar-producto.component';
import { MostrarInventarioComponent } from './components/mostrar-inventario/mostrar-inventario.component';
import { PanelInventarioComponent } from './components/panel-inventario/panel-inventario.component';
import { MostrarEntradasComponent } from './components/mostrar-entradas/mostrar-entradas.component';
import { InventarioRoutingModule } from './inventario-routing.module';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { ConfirmDialogComponent } from './components/shared/confirm-dialog/confirm-dialog.component';
import { EditarInventarioComponent } from './components/editar-inventario/editar-inventario.component';
import { EditarEntradaPesoComponent } from './components/editar-entrada-peso/editar-entrada-peso.component';
import { BuscarSalidasComponent } from './components/buscar-salidas/buscar-salidas.component';
import { RegistrarEntradaPesoComponent } from './components/registrar-entrada-peso/registrar-entrada-peso.component';

// Componentes inventario

@NgModule({
  declarations: [
    RegistrarProductoComponent,
    RegistrarEntradaPesoComponent,
    MostrarInventarioComponent,
    PanelInventarioComponent,
    MostrarEntradasComponent,
    EditarInventarioComponent,
    ConfirmDialogComponent,
    EditarEntradaPesoComponent,
    BuscarSalidasComponent,
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
    MatDialogModule,
    MatButtonModule,

    NgxBarcode6Module,
    ToastrModule.forRoot(),


  ]
})
export class InventarioModule { }