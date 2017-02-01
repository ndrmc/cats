using System.Collections.Generic;


namespace Cats.Services.Transaction
{
    public class TranscationTypeService : ITranscationTypeService
    {
        private readonly Cats.Data.UnitWork.IUnitOfWork _unitOfWork;

        public TranscationTypeService(Cats.Data.UnitWork.IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Models.TransactionType FindById(int id)
        {
            return _unitOfWork.TransactionTypeRepository.FindById(id);
        }

        public List<Models.TransactionType> GetAllTranscationType()
        {
            return _unitOfWork.TransactionTypeRepository.GetAll();

        }
        public void Dispose()
        {
            _unitOfWork.Dispose();

        }
    }
}
