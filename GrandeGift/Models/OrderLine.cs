﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using BiankaKorban_DiplomaProject.Models;

namespace BiankaKorban_DiplomaProject.Models
{
	public class OrderLine
	{
		public int OrderLineId { get; set; } //PK
		public int OrderId { get; set; } //FK
		public int Quantity { get; set; }

		public int HamperId { get; set; } //FK
	}
}
