import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PanelInventarioComponent } from './components/panel-inventario/panel-inventario.component';
import { MostrarInventarioComponent } from './components/mostrar-inventario/mostrar-inventario.component';
import { RegistrarProductoComponent } from './components/registrar-producto/registrar-producto.component';
import { MostrarEntradasComponent } from './components/mostrar-entradas/mostrar-entradas.component';
import { BuscarSalidasComponent } from './components/buscar-salidas/buscar-salidas.component';
import { RegistrarEntradaPesoComponent } from './components/registrar-entrada-peso/registrar-entrada-peso.component';
import { AuditoriaInventarioComponent } from './components/auditoria-inventario/auditoria-inventario.component';


const routes: Routes = [
  {
    path: '',
    component: PanelInventarioComponent,  
    children: [
      { path: 'auditoria-inventario', component: AuditoriaInventarioComponent },
      { path: 'mostrar-inventario', component: MostrarInventarioComponent },
      { path: 'registrar-entrada', component: RegistrarEntradaPesoComponent },
      { path: 'registrar-producto', component: RegistrarProductoComponent },
      { path: 'mostrar-entradas', component: MostrarEntradasComponent },
      { path: 'buscar-salidas', component: BuscarSalidasComponent },
      { path: '', redirectTo: 'auditoria-inventario', pathMatch: 'full' } 
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InventarioRoutingModule {}