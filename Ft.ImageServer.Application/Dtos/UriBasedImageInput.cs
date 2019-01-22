using Ft.ImageServer.Core.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ft.ImageServer.Application.Dtos
{
    public class UriBasedImageInput : ImageInput, IValidatableObject
    {
        /// <summary>
        /// 图片Uri
        /// </summary>
        [Required]
        public string Uri { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var imageInput = validationContext.ObjectInstance as UriBasedImageInput;
            if (string.IsNullOrWhiteSpace(imageInput.Title))
                yield return new ValidationResult("请提供图片Title");
            else if (string.IsNullOrWhiteSpace(imageInput.Uri))
                yield return new ValidationResult("图片Uri不能为空");
            else if (!imageInput.Uri.IsUrl())
                yield return new ValidationResult("图片Uri格式不正确");
            else if (imageInput.X1.GetValueOrDefault() < 0 || imageInput.X2.GetValueOrDefault() < 0
                || imageInput.Y1.GetValueOrDefault() < 0 || imageInput.Y2.GetValueOrDefault() < 0)
                yield return new ValidationResult("座标偏移量不能小于0");
            else if (imageInput.X2 <= imageInput.X1)
                yield return new ValidationResult("座标偏移量X2不能小于等于X1");
            else if (imageInput.Y2 <= imageInput.Y1)
                yield return new ValidationResult("座标偏移量Y2不能小于等于Y1");
        }
    }
}
