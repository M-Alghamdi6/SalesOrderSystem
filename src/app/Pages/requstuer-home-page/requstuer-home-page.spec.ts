import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RequstuerHomePage } from './requstuer-home-page';

describe('RequstuerHomePage', () => {
  let component: RequstuerHomePage;
  let fixture: ComponentFixture<RequstuerHomePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RequstuerHomePage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RequstuerHomePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
