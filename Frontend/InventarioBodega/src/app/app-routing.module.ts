import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: 'inventario', loadChildren: () => import('./inventario/inventario.module').then(m => m.InventarioModule) },
  { path: '', redirectTo: '/inventario', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
