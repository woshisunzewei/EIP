using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EIP.Common.Business;
using EIP.Common.Core.Extensions;
using EIP.Common.Core.Resource;
using EIP.Common.Core.Utils;
using EIP.Common.Entities;
using EIP.Common.Entities.Dtos;
using EIP.Common.Entities.Tree;
using EIP.System.DataAccess.Config;
using EIP.System.Models.Entities;

namespace EIP.System.Business.Config
{
    /// <summary>
    ///     字典业务逻辑实现
    /// </summary>
    public class SystemDictionaryLogic : AsyncLogic<SystemDictionary>, ISystemDictionaryLogic
    {
        #region 构造函数

        public SystemDictionaryLogic()
        {
            _dictionaryRepository = new SystemDictionaryRepository();
        }

        private readonly ISystemDictionaryRepository _dictionaryRepository;

        public SystemDictionaryLogic(ISystemDictionaryRepository dictionaryRepository)
            : base(dictionaryRepository)
        {
            _dictionaryRepository = dictionaryRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        ///     查询所有字典信息:Ztree格式
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TreeEntity>> GetDictionaryTree()
        {
            return await _dictionaryRepository.GetDictionaryTree();
        }

        /// <summary>
        ///     根据父级查询下级
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        public async Task<IEnumerable<SystemDictionary>> GetDictionariesParentId(IdInput input)
        {
            return await _dictionaryRepository.GetDictionariesParentId(input);
        }

        /// <summary>
        ///     保存字典信息
        /// </summary>
        /// <param name="dictionary">字典信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveDictionary(SystemDictionary dictionary)
        {
            if (dictionary.DictionaryId.IsEmptyGuid())
            {
                dictionary.CanbeDelete = true;
                dictionary.DictionaryId = CombUtil.NewComb();
                return await InsertAsync(dictionary);
            }
            return await UpdateAsync(dictionary);
        }

        /// <summary>
        ///     删除字典及下级数据
        /// </summary>
        /// <param name="input">id</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteDictionary(IdInput input)
        {
            var operateStatus = new OperateStatus();

            //判断该字典是否允许删除:可能是系统定义的字典则不允许删除
            var dictionary = await GetByIdAsync(input.Id);
            if (!dictionary.CanbeDelete)
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = Chs.CanotDelete;
                return operateStatus;
            }
            //是否具有子项
            IEnumerable<SystemDictionary> dictionaries = await GetDictionariesParentId(input);
            if (dictionaries.Any())
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format(Chs.Error, ResourceSystem.具有下级项);
                return operateStatus;
            }
            return await DeleteAsync(input.Id);
        }

        /// <summary>
        ///     根据字典代码获取对应下级值
        /// </summary>
        /// <param name="code">代码值</param>
        /// <returns></returns>
        public async Task<IEnumerable<SystemDictionary>> GetDictionaryByCode(string code)
        {
            return await _dictionaryRepository.GetDictionaryByCode(code);
        }

        /// <summary>
        ///     检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        public async Task<OperateStatus> CheckDictionaryCode(CheckSameValueInput input)
        {
            var operateStatus = new OperateStatus();
            if (await _dictionaryRepository.CheckDictionaryCode(input))
            {
                operateStatus.ResultSign = ResultSign.Error;
                operateStatus.Message = string.Format(Chs.HaveCode, input.Param);
            }
            else
            {
                operateStatus.ResultSign = ResultSign.Successful;
                operateStatus.Message = Chs.CheckSuccessful;
            }
            return operateStatus;
        }

        #endregion


        /// <summary>
        /// 根据代码获取字典树
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TreeEntity>> GetDictionaryTreeByCode(string code)
        {
            return await _dictionaryRepository.GetDictionaryTreeByCode(code);
        }

        /// <summary>
        ///     批量生成代码
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> GeneratingCode()
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                //获取所有字典树
                var dics = (await GetAllEnumerableAsync()).ToList();

                var topDics = dics.Where(w => w.ParentId == Guid.Empty);
                foreach (var dic in topDics)
                {
                    dic.Code = PinYinUtil.GetFirst(dic.Name);
                    await UpdateAsync(dic);
                    await GeneratingCodeRecursion(dic, dics.ToList(), "");
                }
            }
            catch (Exception ex)
            {
                operateStatus.Message = string.Format(Chs.Error, ex.Message);
                return operateStatus;
            }
            operateStatus.Message = Chs.Successful;
            operateStatus.ResultSign = ResultSign.Successful;
            return operateStatus;
        }

        /// <summary>
        /// 递归获取代码
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="dictionaries"></param>
        /// <param name="generationCode"></param>
        private async Task GeneratingCodeRecursion(SystemDictionary dictionary, IList<SystemDictionary> dictionaries, string generationCode)
        {
            string emp = PinYinUtil.GetFirst(dictionary.Name);
            //获取下级
            var nextDic = dictionaries.Where(w => w.ParentId == dictionary.DictionaryId).ToList();
            if (nextDic.Any())
            {
                emp = generationCode.IsNullOrEmpty() ? emp : generationCode + "_" + emp;
            }
            foreach (var dic in nextDic)
            {
                dic.Code = emp + "_" + PinYinUtil.GetFirst(dic.Name);
                await UpdateAsync(dic);
                await GeneratingCodeRecursion(dic, dictionaries, emp);
            }
        }
    }
}