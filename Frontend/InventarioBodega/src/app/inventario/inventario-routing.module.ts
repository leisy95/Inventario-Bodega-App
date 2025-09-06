import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PanelInventarioComponent } from './panel-inventario/panel-inventario.component';
import { MostrarInventarioComponent } from './mostrar-inventario/mostrar-inventario.component';
import { MostrarVistaComponent } from './mostrar-vista/mostrar-vista.component';
import { RegistrarProductoComponent } from './registrar-producto/registrar-producto.component';
import { MostrarEntradasComponent } from './mostrar-entradas/mostrar-entradas.component';

const routes: Routes = [
  {
    path: '',
    component: PanelInventarioComponent,  
    children: [
      { path: 'mostrar-inventario', component: MostrarInventarioComponent },
      { path: 'mostrar-vista', component: MostrarVistaComponent },
      { path: 'registrar-producto', component: RegistrarProductoComponent },
      { path: 'mostrar-entradas', component: MostrarEntradasComponent },
      { path: '', redirectTo: 'mostrar-inventario', pathMatch: 'full' } // uta por defecto
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InventarioRoutingModule {}