﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get ; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }

        public BaseSpecifications() {

           // Includes = new List<Expression<Func<T, object>>>();
        }
        public BaseSpecifications(Expression<Func<T, bool>> criteriaExepression) {

            Criteria = criteriaExepression;
           // Includes = new List<Expression<Func<T, object>>>();

        }
        public void AddOrderBy(Expression<Func<T, object>> orderByExpression) {
        OrderBy=orderByExpression;
        }


        public void AddOrderByDescending(Expression<Func<T, object>> orderByDecExpression)
        {
            OrderBy = orderByDecExpression;
        }

        public void ApplyPagination(int skip, int take) {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        
        }


    }
}
