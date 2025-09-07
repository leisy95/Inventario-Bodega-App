import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { InventarioItem } from '../../models/inventario-item.model';
import { InventarioItemService } from '../../services/inventario-item.service';
import { ToastrService } from 'ngx-toastr';
import { Subject, Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-buscar-salidas',
  templateUrl: './buscar-salidas.component.html',
  styleUrls: ['./buscar-salidas.component.css']
})
export class BuscarSalidasComponent implements OnInit, OnDestroy {
  filtroReferencia: string = '';
  private searchSubject = new Subject<string>();
  private subscription!: Subscription;

  dataSource = new MatTableDataSource<InventarioItem>([]);
  displayedColumns: string[] = ['referenciaPeso', 'fechaRegistroItem', 'pesoActual', 'acciones'];

  constructor(
    private inventarioItemService: InventarioItemService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    // Escucha los cambios del buscador con 400ms de espera
    this.subscription = this.searchSubject
      .pipe(debounceTime(400))
      .subscribe(value => this.buscar(value));
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  // Capturar cambios en tiempo real
  onSearchChange(value: string): void {
    this.searchSubject.next(value);
  }

  buscar(valor: string): void {
    if (!valor.trim()) {
      this.dataSource.data = [];
      return;
    }

    this.inventarioItemService.getEntradas().subscribe({
      next: (data) => {
        const resultados = data.filter(item =>
          item.referenciaPeso.toLowerCase().includes(valor.toLowerCase())
        );
        this.dataSource.data = resultados;

        if (!resultados.length) {
          this.toastr.info('No se encontraron registros para la referencia');
        }
      },
      error: () => this.toastr.error('Error al buscar en inventario')
    });
  }

  enviar(item: InventarioItem): void {
    this.toastr.success(`Enviada salida para ${item.referenciaPeso}`, 'Salida registrada');
  }
}