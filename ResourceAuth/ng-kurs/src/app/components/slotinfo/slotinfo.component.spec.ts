import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SlotinfoComponent } from './slotinfo.component';

describe('SlotinfoComponent', () => {
  let component: SlotinfoComponent;
  let fixture: ComponentFixture<SlotinfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SlotinfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SlotinfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
