import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegistrarProductoComponent } from './inventario/registrar-producto/registrar-producto.component';
import { MostrarVistaComponent } from './inventario/mostrar-vista/mostrar-vista.component';


const routes: Routes = [
  { path: 'registrar-producto', component: RegistrarProductoComponent },
  { path: 'mostrar-vista', component: MostrarVistaComponent },
  { path: '', redirectTo: 'registrar-producto', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
