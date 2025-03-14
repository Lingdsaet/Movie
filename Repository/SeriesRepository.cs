﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movie.Models;

namespace Movie.Repository
{
    public class SeriesRepository
    {
        private readonly movieDB _context;
        public SeriesRepository(movieDB context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Series>> GetAllAsync()
        {
            return await _context.Series.ToListAsync();
        }

        public Task<Series> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Series entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Series entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}