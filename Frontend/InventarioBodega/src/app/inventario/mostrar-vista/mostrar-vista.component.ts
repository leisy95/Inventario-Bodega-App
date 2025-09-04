import { Component, OnInit } from '@angular/core';
import { Inventario, InventarioService } from '../../services/inventario.service';
import { FormControl } from '@angular/forms';
import { map, Observable, startWith } from 'rxjs';

@Component({
  selector: 'app-mostrar-vista',
  templateUrl: './mostrar-vista.component.html',
  styleUrls: ['./mostrar-vista.component.css']
})
export class MostrarVistaComponent implements OnInit {
  referenciaControl = new FormControl('');
  referencias: string[] = [];
  referenciasFiltradas!: Observable<string[]>;
  pesoIngresado: number | null = null;
  inventario: Inventario | null = null;
  pesoActual: number | null = null;
  fechaImpresion: Date = new Date();

  constructor(private inventarioService: InventarioService) { }

  ngOnInit(): void {
    // Trae todas las referencias al inicio
    this.filtrarReferencias();
    this.configurarEventoImpresion();
  }

  ngOnDestroy(): void {
    this.removerEventoImpresion(); // 
  }

  //Fecha de Impresion
  actualizarFechaImpresion(): void {
    this.fechaImpresion = new Date();
  }

  private beforePrintListener = () => {
    this.actualizarFechaImpresion();
  };

  private configurarEventoImpresion(): void {
    window.addEventListener('beforeprint', this.beforePrintListener);
  }

  private removerEventoImpresion(): void {
    window.removeEventListener('beforeprint', this.beforePrintListener);
  }

  filtrarReferencias() {
    this.inventarioService.getInventarios().subscribe({
      next: (res) => {
        this.referencias = res.map(i => i.referencia);

        this.referenciasFiltradas = this.referenciaControl.valueChanges.pipe(
          startWith(''),
          map(value => this._filter(value || ''))
        );
      }
    });
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();
    return this.referencias.filter(ref =>
      ref.toLowerCase().startsWith(filterValue)
    );
  }

  buscar() {
    this.pesoActual = null;

    const ref = this.referenciaControl.value;
    if (!ref || !ref.trim()) return;

    this.inventarioService.getByReferencia(ref).subscribe({
      next: (res) => {
        this.inventario = res;
        this.pesoIngresado = null;
      },
      error: () => {
        this.inventario = null;
      }
    });
  }

  agregarPeso() {
    const peso = Number(this.pesoIngresado || 0);
    if (!peso) return;
    this.pesoActual = peso;

    // Guardar en BD
    if (this.referenciaControl.value) {
      this.inventarioService.ingresarPeso(this.referenciaControl.value, peso).subscribe({
        next: () => {
          this.pesoActual = peso;
        },
        error: (err) => console.error(err)
      });
    } else {
      console.warn("No hay referencia seleccionada, no se guarda en BD");
    }
  }

  /* Imprimir etiqueta */
  imprimirEtiqueta() {
    this.actualizarFechaImpresion();
    window.print();
  }

  getReferenciaParaBarcode(): string {
    return this.inventario?.referencia?.replace(/[^A-Za-z0-9]/g, '') || '';
  }
}