import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MostrarEntradasComponent } from './mostrar-entradas.component';

describe('MostrarEntradasComponent', () => {
  let component: MostrarEntradasComponent;
  let fixture: ComponentFixture<MostrarEntradasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MostrarEntradasComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MostrarEntradasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
