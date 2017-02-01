using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;

namespace cmmi.review.data
{
    public class UnitOfWork : IDisposable
    {
        #region Private member variables...

        private DataEntities _context = null;
        private GenericRepository<User> _userRepository;
        private GenericRepository<Restaurant> _restaurantRepository;
        private GenericRepository<Review> _reviewRepository;

        #endregion

        public UnitOfWork()
        {
            _context = new DataEntities();
        }

        public GenericRepository<User> UserRepository
        {
            get
            {
                if (this._userRepository == null)
                    this._userRepository = new GenericRepository<User>(_context);
                return _userRepository;
            }
        }

        public GenericRepository<Restaurant> RestaurantRepository
        {
            get
            {
                if (this._restaurantRepository == null)
                    this._restaurantRepository = new GenericRepository<Restaurant>(_context);
                return _restaurantRepository;
            }
        }

        public GenericRepository<Review> ReviewRepository
        {
            get
            {
                if (this._reviewRepository == null)
                    this._reviewRepository = new GenericRepository<Review>(_context);
                return _reviewRepository;
            }
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                // Normally log this to an error log or some form of output.  Disconnecting for expediency sake
                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw e;
            }

        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
