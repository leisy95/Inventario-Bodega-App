import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditarEntradaPesoComponent } from './editar-entrada-peso.component';

describe('EditarEntradaPesoComponent', () => {
  let component: EditarEntradaPesoComponent;
  let fixture: ComponentFixture<EditarEntradaPesoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EditarEntradaPesoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditarEntradaPesoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
